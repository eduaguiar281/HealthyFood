using System;
using System.Collections.Generic;

namespace HowToDevelop.Core.Infraestrutura
{
    public class PagedList<T> where T : class
    {
        public PagedList(IEnumerable<T> lista, in int tamanhoDaPagina, in int numeroDaPagina, in int totalRegistros)
        {
            Lista = lista;
            TamanhoDaPagina = tamanhoDaPagina;
            NumeroDaPagina = numeroDaPagina;
            TotalRegistros = totalRegistros;
            NumeroDePaginas = Convert.ToInt32(Math.Ceiling((decimal)(totalRegistros / tamanhoDaPagina)));
        }

        public IEnumerable<T> Lista { get; }
        public int TotalRegistros { get; }
        public int NumeroDaPagina { get; }
        public int TamanhoDaPagina { get; }
        public int NumeroDePaginas { get; }
    }

    public static class PagedListExtensions
    {
        public static PagedList<T> ToPageList<T>(this IEnumerable<T> lista, in int tamanhoDaPagina, 
            in int numeroDaPagina, in int totalRegistro) where T: class
        {
            return new PagedList<T>(lista, tamanhoDaPagina, numeroDaPagina, totalRegistro);
        }
    }

}
