using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bot_De_Telegram
{
    public class CuentaPesos
    {
        public string Nombre { get; set; }
        public string Moneda { get; set; }
        public List<Productos> Productos { get; set; }

        Random random = new Random();
        public CuentaPesos(Usuario usuario, string pesos)
        {
            string monedilla = pesos.ToLower();
            if (monedilla == "pesos")
            {
                this.Nombre = "Caja de ahorros pesos " + random.Next(1, 9);
                Productos = new List<Productos>();
            }
            else if (monedilla == "dolares")
            {
                this.Nombre = "Caja de ahorros dolares " + random.Next(1, 9);
                Productos = new List<Productos>();
            }
        }

        public void AgregarProducto(Productos productoNuevo)
        {
            Productos.Add(productoNuevo);
        }

        public string MostrarProductos()
        {
            return string.Join("\n", Productos.Select(p => p.ProductoListo()));
        }

    }
}
