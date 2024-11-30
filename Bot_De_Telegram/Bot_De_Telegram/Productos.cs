using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace Bot_De_Telegram
{
    public class Productos
    {
        string provedor {  get; set; }
        string name {  get; set; }
        int id { get; set; }
        double price { get; set; }
        string signo { get; set; }

        public Productos (string provedor, string name, int id, double price, string signoMoneda)
        {
            this.provedor = provedor;
            this.name = name;
            this.id = id;
            this.price = price;
            this.signo = signoMoneda;
        }

        public string ProductoListo() 
        {
            return $"{provedor} - {name} - {price}{signo}\n";
        }
    }
}
