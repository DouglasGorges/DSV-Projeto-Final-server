using System.Collections.Generic;
using System.Linq;
using server.Models;

namespace server.Utils
{
    public class CategoriaUtil
    {
        public bool hasMatch(List<Categoria> lista_A, List<Categoria> lista_B)
        {
            return lista_A.Any(x => lista_B.Any(y => y == x));
        }
    }
}