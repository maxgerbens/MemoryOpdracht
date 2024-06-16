namespace TestWPFCode;

public class Program
{
    static void Main(string[] args)
    {
        foreach (var card in MemoryCard.Images)
        {
            Console.WriteLine(card.Length);
        }
    }
}