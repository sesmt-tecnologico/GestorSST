namespace Gestor.Domain.Enums
{
    public enum StatusEmpregado
    {
        Livre = 0, // cinza                 => sem nenhuma admissão ativa
        Admitido = 1, // azul claro         => com admissão ativa, mas sem nenhuma alocação
        Alocado = 2, // azul escuro         => com admissão ativa, alocação ativa mas sem documentação aprovada
        Liberado = 3, // verde              => com admissão ativa, todas alocações ativas e toda documentação aprovada
        LiberadoAlocado = 4, // azul escuro => com admissão ativa, AO MENOS UMA alocação aprovada e AO MENOS UMA alocação ativa mas sem documentação aprovada
        LiberadoIrregular = 5, // vermelho  => com admissão ativa, AO MENOS UMA alocação aprovada e AO MENOS UMA alocação, outrora aprovada, agora pendente (documento vencido, por exemplo). Neste caso, consultar a tela de alocações para maiores detalhes
        Irregular = 9 // vermelho           => com admissão ativa e TODAS alocações, outrora aprovadas, agora pendentes (documentos vencidos, por exemplo). Neste caso, consultar a tela de alocações para maiores detalhes
    }
}