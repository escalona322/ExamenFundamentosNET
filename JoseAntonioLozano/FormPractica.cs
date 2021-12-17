using JoseAntonioLozano.Context;
using JoseAntonioLozano.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProyectoADO
{
    public partial class FormPractica : Form
    {
        ClientesContext context;
        public FormPractica()
        {
            InitializeComponent();
            this.context = new ClientesContext();
            List<Cliente> clientes = this.context.getClientes();
            foreach (Cliente cli in clientes)
            {
                this.cmbclientes.Items.Add(cli.Empresa);
            }
        }

        private void cmbclientes_SelectedIndexChanged(object sender, EventArgs e)
        {
            string Empresa = this.cmbclientes.SelectedItem.ToString();

            Cliente cli = new Cliente();
            cli = this.context.FindClienteByEmpresa(Empresa);

            this.txtcargo.Text = cli.Cargo;
            this.txtciudad.Text = cli.Ciudad;
            this.txtcontacto.Text = cli.Contacto;
            this.txtempresa.Text = cli.Empresa;
            this.txttelefono.Text = cli.Telefono;

            List<Pedido> pedidos = new List<Pedido>();
            pedidos = this.context.GetPedidosByCodCli(cli.CodigoCliente);
            this.lstpedidos.Items.Clear();
            foreach(Pedido pedido in pedidos)
            {
                this.lstpedidos.Items.Add(pedido.CodigoPedido);
            }
        }



        private void btnmodificarcliente_Click(object sender, EventArgs e)
        {
            string codcli = this.cmbclientes.SelectedItem.ToString();
            codcli = codcli.Substring(0, 3);
            string empresa = this.txtempresa.Text;
            string cargo = this.txtcargo.Text;
            string ciudad = this.txtciudad.Text;
            string contacto = this.txtcontacto.Text;
            string telefono = this.txttelefono.Text;

            int update = this.context.UpdateCliente(codcli, empresa, contacto, cargo, ciudad, telefono);
            this.cmbclientes.Items.Clear();
            List<Cliente> clientes = this.context.getClientes();
            foreach (Cliente cli in clientes)
            {
                this.cmbclientes.Items.Add(cli.Empresa);
            }
        }

        private void lstpedidos_SelectedIndexChanged(object sender, EventArgs e)
        {
            string codpedido = this.lstpedidos.SelectedItem.ToString();

            Pedido pedidoseleccionado = new Pedido();

            pedidoseleccionado = this.context.GetPedidosByCodPed(codpedido);

            this.txtcodigopedido.Text = pedidoseleccionado.CodigoPedido;
            this.txtfechaentrega.Text = pedidoseleccionado.FechaEntrega;
            this.txtformaenvio.Text = pedidoseleccionado.FormaEnvio;
            this.txtimporte.Text = pedidoseleccionado.Importe.ToString();
        }

        private void btnnuevopedido_Click(object sender, EventArgs e)
        {
            String codped = this.txtcodigopedido.Text;
            String codcli = this.cmbclientes.SelectedItem.ToString();
            codcli = codcli.Substring(0, 3);
            string fechaentrega = this.txtfechaentrega.Text;
            string formaenvio = this.txtformaenvio.Text;
            int importe = int.Parse(this.txtimporte.Text);

            int insertar = this.context.InsertarPedido(codped, codcli, fechaentrega, formaenvio, importe);

            List<Pedido> pedidos = new List<Pedido>();
            pedidos = this.context.GetPedidosByCodCli(codcli);
            this.lstpedidos.Items.Clear();
            foreach (Pedido pedido in pedidos)
            {
                this.lstpedidos.Items.Add(pedido.CodigoPedido);
            }
        }

        private void btneliminarpedido_Click(object sender, EventArgs e)
        {
            String codped = this.lstpedidos.SelectedItem.ToString();
            String codcli = this.cmbclientes.SelectedItem.ToString();
            codcli = codcli.Substring(0, 3);
            int eliminado = this.context.EliminarPedido(codped);

            List<Pedido> pedidos = new List<Pedido>();
            pedidos = this.context.GetPedidosByCodCli(codcli);
            this.lstpedidos.Items.Clear();
            foreach (Pedido pedido in pedidos)
            {
                this.lstpedidos.Items.Add(pedido.CodigoPedido);
            }
        }
    }
}
