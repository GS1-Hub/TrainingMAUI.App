using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System.Data;

namespace Training;

public partial class ClientsPage : ContentPage
{
    private readonly IConfiguration _configuration;

    public ClientsPage(IConfiguration configuration)
	{
		InitializeComponent();
		_configuration = configuration;
	}
    protected override void OnAppearing()
    {
        base.OnAppearing();
        var clientes = GetClientes();
        ClientsCollectionView.ItemsSource = clientes;
    }

    private List<Dictionary<string, object?>> GetClientes()
    {
        var clientes = new List<Dictionary<string, object?>>();
        string connectionString = _configuration["ConnectionStrings:Default"]
            ?? throw new InvalidOperationException("Connection string não encontrada");

        using (var conn = new MySqlConnection(connectionString))
        {
            conn.Open();
            string query = @"SELECT * FROM clientes";

            using var cmd = new MySqlCommand(query, conn);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                clientes.Add(new Dictionary<string, object?>
                {
                    ["id"] = reader.GetInt32("id"),
                    ["nome"] = reader.GetString("nome"),
                    ["cidade"] = reader.GetString("cidade"),
                    ["data_registo"] = reader.GetDateTime("data_registo")
                });
            }
        }
        return clientes;
    }
}