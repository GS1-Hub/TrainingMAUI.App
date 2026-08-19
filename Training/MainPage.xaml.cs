using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;

namespace Training
{
    public partial class MainPage : ContentPage
    {
        private readonly IConfiguration _configuration;

        public MainPage(IConfiguration configuration)
        {
            InitializeComponent();
            _configuration = configuration;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            var produtos = GetProdutos();
        }

        public List<Dictionary<string, object?>> GetProdutos()
        {
            var produtos = new List<Dictionary<string, object?>>();
            string connectionString = _configuration["ConnectionStrings:Default"]
                ?? throw new InvalidOperationException("Connection string não encontrada");

            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = @"
                                SELECT 
                                    c.id AS cliente_id, 
                                    c.nome AS cliente_nome, 
                                    e.id AS encomenda_id, 
                                    e.data_encomenda
                                FROM clientes c
                                INNER JOIN encomendas e ON c.id = e.cliente_id
                                ORDER BY c.nome";

                using var cmd = new MySqlCommand(query, conn);
                using var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    produtos.Add(new Dictionary<string, object?>
                    {
                        ["cliente_id"] = reader.GetInt32("cliente_id"),
                        ["cliente_nome"] = reader.GetString("cliente_nome"),
                        ["encomenda_id"] = reader.IsDBNull(reader.GetOrdinal("encomenda_id"))
                            ? null : reader.GetInt32("encomenda_id"),
                                        ["data_encomenda"] = reader.IsDBNull(reader.GetOrdinal("data_encomenda"))
                            ? null : reader.GetDateTime("data_encomenda")
                    });
                }
            }
            return produtos;
        }

        private List<string> ListStrings()
        {
            List<string> listShow = [];

            listShow.AddRange("Olá", "Adeus", "Bonjour");

            if (listShow.Count < 0)
                return [];

            return listShow;
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            List<string> list = ListStrings();

            LabelListString.Text = String.Join("\n", list);
            LabelListString.FontSize = 40;
            LabelListString.FontAttributes = FontAttributes.Bold;

            LabelListString2.Text = String.Join("\n", list);
            LabelListString2.FontSize = 40;
            LabelListString2.FontAttributes = FontAttributes.Bold;

            LabelListString3.Text = String.Join("\n", list);
            LabelListString3.FontSize = 40;
            LabelListString3.FontAttributes = FontAttributes.Bold;
        }
    }
}