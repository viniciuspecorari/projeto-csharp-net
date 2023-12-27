using System.Drawing;
using System.Threading;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Iniciando redimensionador");

        // Rodando o programa em uma thread diferente
        Thread thread = new Thread(Redimensionar);
        thread.Start();



    }

    static void Redimensionar()
    {        
        
        string diretorio_entrada = "Arquivos_Entrada";
        string diretorio_redimensionado = "Arquivos_Redimensionado";
        string diretorio_finalizado = "Arquivo_Finalizados";

        Console.WriteLine(diretorio_entrada);
        Console.WriteLine(diretorio_redimensionado);
        Console.WriteLine(diretorio_finalizado);

        if (!Directory.Exists(diretorio_entrada))
        {
            Directory.CreateDirectory(diretorio_entrada);
        }
        if (!Directory.Exists(diretorio_redimensionado))
        {            
            Directory.CreateDirectory(diretorio_redimensionado);
        }
        if (!Directory.Exists(diretorio_finalizado))
        {
            Directory.CreateDirectory(diretorio_finalizado);
        }

        FileStream fileStream;
        FileInfo fileInfo;

        while (true)
        {
            // Meu programa vai olhar para a pasta de entrada
            // Se tiver arquivo, ele irá redimensionar
            var arquivos_entrada = Directory.EnumerateFiles(diretorio_entrada);            

            // Ler o tamanho que irá redimensionar
            int novaAltura = 200;

            foreach (var arquivo in arquivos_entrada)
            {                       
                fileStream = new FileStream(arquivo, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
                fileInfo = new FileInfo(arquivo);

                string caminho =    Environment.CurrentDirectory + @"\" + 
                                    diretorio_redimensionado + @"\" +
                                    DateTime.Now.Millisecond.ToString() + fileInfo.Name;

                // Redimensionar + Copia os arquivos redimensionados para a pasta de redimensionados
                Redimensionador(Image.FromStream(fileStream), novaAltura, caminho);

                // Fechar o arquivo
                fileStream.Close();

                string caminhoFinalizado = Environment.CurrentDirectory + @"\" + diretorio_finalizado + @"\" + fileInfo.Name;

                // Move o arquivo de entrada para a pasta de finalizados
                fileInfo.MoveTo(caminhoFinalizado);
            }

            Thread.Sleep(new TimeSpan(0, 0, 5));
        }
    }

    static void Redimensionador(Image imagem, int altura, string caminho)
    {
        double ratio = (double)altura / imagem.Height;
        int novaLargura = (int)(imagem.Width * ratio);
        int novaAltura = (int)(imagem.Height * ratio);

        Bitmap novaImage = new Bitmap(novaLargura, novaAltura);
        using(Graphics g = Graphics.FromImage(novaImage))
        {
            g.DrawImage(imagem, 0, 0, novaLargura, novaAltura);
        }

        novaImage.Save(caminho);
        imagem.Dispose();

    }
}