namespace HeadWars
{
#if WINDOWS
	static class Program
	{
		static void Main(string[] args)
		{
			using (HeadWars game = new HeadWars())
			{
				game.Run();
			}
		}
	}
#endif
}