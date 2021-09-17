using System;
namespace server.Models
{
    public class ContaCorrente
    {
        public ContaCorrente() => CriadoEm = DateTime.Now;

        public int Id { get; set;}
        public string Nome { get; set;}
        public double Saldo { get; set;}
        public DateTime CriadoEm { get; set;}

        public override string ToString() =>
            $"Nome: {Nome} | Preco: {Saldo:C2} | Criado em: {CriadoEm}";
   
    }
}