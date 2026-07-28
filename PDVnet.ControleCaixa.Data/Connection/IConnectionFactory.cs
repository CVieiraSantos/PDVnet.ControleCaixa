using Microsoft.Data.SqlClient;

namespace PDVnet.ControleCaixa.Data.Connection
{
    public interface IConnectionFactory
    {
        SqlConnection Create();
    }
}
