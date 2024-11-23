using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistema.Negocio.DTO
{
    public class RespuestaDTO<T>
    {
        public RespuestaDTO()
        {
        }

        public RespuestaDTO(bool estado, string mensaje, T data)
        {
            Estado = estado;
            Mensaje = mensaje;
            Data = data;
        }

        public bool Estado { get; set; }
        public string Mensaje { get; set; }
        public T Data { get; set; }
    }
}
