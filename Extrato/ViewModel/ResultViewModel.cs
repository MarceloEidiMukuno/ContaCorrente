namespace ContaCorrente.ApiExtrato.ViewModels
{

    public class ResultViewModel<T>
    {

        public ResultViewModel(T data, List<string> erros)
        {
            Data = data;
            Erros = erros;
        }
        public ResultViewModel(T data)
        {
            Data = data;
        }

        public ResultViewModel(List<string> erros)
        {
            Erros = erros;
        }

        public ResultViewModel(string erros)
        {
            Erros.Add(erros);
        }
        public T Data { get; private set; }
        // Já inicializando a lista
        public List<string> Erros { get; private set; } = new();

    }
}