using JoseAntonioLozano.Model;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

#region PROCEDURES
//create procedure clientes_by_empresa
//(@empresa nvarchar(50))
//as 
// select* from clientes
//    where empresa = @empresa
//go
//        create procedure updatecliente
//(@CodCliente nvarchar(50),@empresa nvarchar(50),@contacto nvarchar(50),
//@Cargo nvarchar(50),@Ciudad nvarchar(50),@Telefono nvarchar(50))
//as 
//	update clientes set CodigoCliente = @CodCliente,
//    Empresa = @empresa,
//    Contacto = @contacto,
//    Cargo = @Cargo,
//    Ciudad = @Ciudad,
//    telefono = @Telefono

//    where CodigoCliente = @CodCliente
//go

#endregion
namespace JoseAntonioLozano.Context
{
    public class ClientesContext
    {
        private SqlConnection cn;
        private SqlCommand com;
        private SqlDataReader reader;

        public ClientesContext()
        {
            string cadenaconexion = "Data Source=LOCALHOST;Initial Catalog=EXAMEN;Persist Security Info=True;User ID=sa;Password=MCSD2021";
            this.cn = new SqlConnection(cadenaconexion);
            this.com = new SqlCommand();
            this.com.Connection = this.cn;
        }

        public List<Cliente> getClientes()
        {
            string sql = "select * from clientes";
            this.com.CommandType = System.Data.CommandType.Text;
            this.com.CommandText = sql;
            this.cn.Open();
            this.reader = this.com.ExecuteReader();

            List<Cliente> clientes = new List<Cliente>();
            while (this.reader.Read())
            {
                Cliente cliente = new Cliente();

                cliente.CodigoCliente = this.reader["CodigoCliente"].ToString();
                cliente.Empresa = this.reader["Empresa"].ToString();
                cliente.Contacto = this.reader["Contacto"].ToString();
                cliente.Cargo = this.reader["Cargo"].ToString();
                cliente.Ciudad = this.reader["Ciudad"].ToString();
                cliente.Telefono = this.reader["Telefono"].ToString();

                clientes.Add(cliente);
            }

            this.reader.Close();
            this.cn.Close();
           
            return clientes;
        }

        #region PROCEDURECLIENTEPOREMPRESA
        //create procedure clientes_by_empresa
        //(@empresa nvarchar(50))
        //as 
	       // select* from clientes
        //    where empresa = @empresa
        //go
        #endregion
        public Cliente FindClienteByEmpresa(string empresa)
        {
            string sql = "clientes_by_empresa";
            SqlParameter paramEmpresa = new SqlParameter("@empresa", empresa);
            this.com.Parameters.Add(paramEmpresa);
            this.com.CommandType = CommandType.StoredProcedure;
            this.com.CommandText = sql;
            this.cn.Open();
            this.reader = this.com.ExecuteReader();
            this.reader.Read();

            Cliente cliente = new Cliente();

            cliente.CodigoCliente = this.reader["CodigoCliente"].ToString();
            cliente.Empresa = this.reader["Empresa"].ToString();
            cliente.Contacto = this.reader["Contacto"].ToString();
            cliente.Cargo = this.reader["Cargo"].ToString();
            cliente.Ciudad = this.reader["Ciudad"].ToString();
            cliente.Telefono = this.reader["Telefono"].ToString();

            this.com.Parameters.Clear();
            this.reader.Close();
            this.cn.Close();
            
            return cliente;
        }

        #region PROCEDUREUPDATECLIENTE
//        create procedure updatecliente
//(@CodCliente nvarchar(50),@empresa nvarchar(50),@contacto nvarchar(50),
//@Cargo nvarchar(50),@Ciudad nvarchar(50),@Telefono nvarchar(50))
//as 
//	update clientes set CodigoCliente = @CodCliente,
//    Empresa = @empresa,
//    Contacto = @contacto,
//    Cargo = @Cargo,
//    Ciudad = @Ciudad,
//    telefono = @Telefono

//    where CodigoCliente = @CodCliente
//go
        #endregion
        public int UpdateCliente(string codcli, string empresa, string contacto
            ,string cargo, string ciudad, string telefono)
        {
            string CodCliente = codcli;
            string sql = "updatecliente";
            SqlParameter paramEmpresa = new SqlParameter("@empresa", empresa);
            this.com.Parameters.Add(paramEmpresa);
            SqlParameter paramContacto = new SqlParameter("@contacto", contacto);
            this.com.Parameters.Add(paramContacto);
            SqlParameter paramCargo = new SqlParameter("@cargo", cargo);
            this.com.Parameters.Add(paramCargo);
            SqlParameter paramCiudad = new SqlParameter("@ciudad", ciudad);
            this.com.Parameters.Add(paramCiudad);
            SqlParameter paramTelefono = new SqlParameter("@telefono", telefono);
            this.com.Parameters.Add(paramTelefono);
            SqlParameter paramCodCli = new SqlParameter("@CodCliente", CodCliente);
            this.com.Parameters.Add(paramCodCli);
            this.com.CommandType = CommandType.StoredProcedure;
            this.com.CommandText = sql;
            this.cn.Open();

            int modificado = this.com.ExecuteNonQuery();
            this.cn.Close();
            this.com.Parameters.Clear();

            return modificado;
        }

        public List<Pedido> GetPedidosByCodCli(string codcli)
        {
            string sql = "select * from pedidos where CodigoCliente = @codcli";
            SqlParameter paramCodCli = new SqlParameter("@codcli", codcli);
            this.com.Parameters.Add(paramCodCli);
            this.com.CommandType = System.Data.CommandType.Text;
            this.com.CommandText = sql;
            this.cn.Open();
            this.reader = this.com.ExecuteReader();

            List<Pedido> pedidos = new List<Pedido>();
            while (this.reader.Read())
            {
                Pedido ped = new Pedido();

                ped.CodigoPedido = this.reader["CodigoPedido"].ToString();
                ped.CodigoCliente = this.reader["CodigoCliente"].ToString();
                ped.FechaEntrega = this.reader["FechaEntrega"].ToString();
                ped.FormaEnvio = this.reader["FormaEnvio"].ToString();
                ped.Importe = int.Parse(this.reader["Importe"].ToString());

                pedidos.Add(ped);
            }
            this.com.Parameters.Clear();
            this.reader.Close();
            this.cn.Close();

            return pedidos;
        }

        public Pedido GetPedidosByCodPed(string codped)
        {
            string sql = "select * from pedidos where CodigoPedido = @codped";
            SqlParameter paramCodPed = new SqlParameter("@codped", codped);
            this.com.Parameters.Add(paramCodPed);
            this.com.CommandType = System.Data.CommandType.Text;
            this.com.CommandText = sql;
            this.cn.Open();
            this.reader = this.com.ExecuteReader();

            this.reader.Read();
                Pedido ped = new Pedido();

                ped.CodigoPedido = this.reader["CodigoPedido"].ToString();
                ped.CodigoCliente = this.reader["CodigoCliente"].ToString();
                ped.FechaEntrega = this.reader["FechaEntrega"].ToString();
                ped.FormaEnvio = this.reader["FormaEnvio"].ToString();
                ped.Importe = int.Parse(this.reader["Importe"].ToString());

             
            
            this.com.Parameters.Clear();
            this.reader.Close();
            this.cn.Close();

            return ped;
        }

        public int InsertarPedido(string codped, string codcli, 
            string fechaentrega, string formaenvio, int importe)
        {
            string sql = "insert into pedidos values (@codped, @codcli, @fechaent, @formaenvio, @importe)";
            SqlParameter paramCodPed = new SqlParameter("@codped", codped);
            this.com.Parameters.Add(paramCodPed);
            SqlParameter paramCodCli = new SqlParameter("@codcli", codcli);
            this.com.Parameters.Add(paramCodCli);
            SqlParameter paramFechaEnt = new SqlParameter("@fechaent", fechaentrega);
            this.com.Parameters.Add(paramFechaEnt);
            SqlParameter paramFormaEnvio = new SqlParameter("@formaenvio", formaenvio);
            this.com.Parameters.Add(paramFormaEnvio);
            SqlParameter paramImporte = new SqlParameter("@importe", importe);
            this.com.Parameters.Add(paramImporte);

            this.com.CommandType = CommandType.Text;
            this.com.CommandText = sql;
            this.cn.Open();

            int insertado = this.com.ExecuteNonQuery();
            this.cn.Close();
            this.com.Parameters.Clear();

            return insertado;
        }

        public int EliminarPedido(string codpedido)
        {
            string sql = "delete from pedidos where CodigoPedido = @codped";
            SqlParameter paramCodPed = new SqlParameter("@codped", codpedido);
            this.com.Parameters.Add(paramCodPed);

            this.com.CommandType = CommandType.Text;
            this.com.CommandText = sql;
            this.cn.Open();

            int eliminado = this.com.ExecuteNonQuery();
            this.cn.Close();
            this.com.Parameters.Clear();

            return eliminado;
        }



    }
}
