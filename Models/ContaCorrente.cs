using System;
namespace server.Models
{
    public class ContaCorrente
    {
        public ContaCorrente() { CriadoEm = DateTime.Now; Ativo = true; }

        public int Id { get; set;}
        public string Nome { get; set;}
        public double SaldoInicial { get; set;}
        public bool Ativo { get; set;}
        public DateTime CriadoEm { get; set;}

        public override string ToString() =>
            $"Nome: {Nome} | SaldoInicial: {SaldoInicial:C2} | Criado em: {CriadoEm}";
   
    }
}