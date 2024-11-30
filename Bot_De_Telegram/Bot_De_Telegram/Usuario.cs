using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bot_De_Telegram
{
    public class Usuario
    {
        public string Nombre {  get; set; }
        public string Contraseña { get; set; }

        public Usuario(string name, string clave)
        {
            this.Nombre = name;
            this.Contraseña = clave;
        }
    }
}
