namespace MontyHallProof
{
    /*
        The law of large numbers states that as a sample size increases, the sample mean will converge to the theoretical
        population mean. For example, if we flip a fair coin that has a 50% probability of landing on heads, 
        the law of large numbers says that as we flip more and more coins, the observed percentage of heads 
        will get closer and closer to 50%.

        We can use this concept to prove that our calculated probability of an event is correct. 
        Let's say we calculate that the probability of rolling a 6 on a fair die is 1/6 or about 16.67%. 
        To prove this, we conduct an experiment rolling a die a large number of times, say 10,000 rolls. 
        We record the number of times a 6 is rolled. If our calculation is correct, we would expect the 
        observed percentage of 6s to be close to 16.67% according to the law of large numbers.

        After rolling 10,000 times, we observe 1,650 6s rolled. This is 16.5% of all rolls, very close to the 
        expected 16.67%. As we increase the number of repetitions, we would expect the observed percentage to 
        converge even closer. This experiment provides evidence, based on the law of large numbers, that the 
        calculated 1/6 probability of rolling a 6 is correct. The more trials performed, the stronger the proof becomes.

        In summary, the law of large numbers allows us to empirically prove probability calculations. 
        By conducting an experiment across many repetitions, we expect the observed results to align with 
        the theoretical probability based on the law of large numbers. This provides data-driven proof of the 
        underlying probability math.
     */
    internal class Program
    {
        private const int NUM_TRIALS = 10_000_000;
        private static readonly IReadOnlyList<int[]> _possibilities = new List<int[]> {
          new[] {0,0,1},
          new[] {0,1,0},
          new[] {1,0,0}
        };
        private static readonly Random _rnd = new Random();

        static void Main(string[] args)
        {
            //first the case without switching
            int[] resultsWithoutSwitching = new int[NUM_TRIALS];
            for (int i = 0; i < NUM_TRIALS; i++)
            {
                var fact = _possibilities[_rnd.Next(0, 3)].ToList();
                //we can short circuit the rest of the logic as we're not switching
                resultsWithoutSwitching[i] = fact[_rnd.Next(0, 3)];
            }

            //now the case with switching
            int[] resultsWithSwitching = new int[NUM_TRIALS];
            for (int i = 0; i < NUM_TRIALS; i++)
            {
                int[] fact = _possibilities[_rnd.Next(0, 3)];
                int initialChoiceIndex = _rnd.Next(0, 3);
                var remainingDoors = fact.Select((x, i) => (x, i))
                    .Where((x, i) => i != initialChoiceIndex).ToList();
                var possibleDoorsToReveal = remainingDoors.Where((x, i) => x.x == 0).ToList();
                var doorToReveal = possibleDoorsToReveal[_rnd.Next(0, possibleDoorsToReveal.Count)];
                //as we're always switching, the final choice would be the one that's not the initial choice
                //and not the one that was revealed by choosing a random that's a goat and was not the initial choice
                resultsWithSwitching[i] = remainingDoors.Where((x, i) => x.i != doorToReveal.i
                    && x.i != initialChoiceIndex).First().x;
            }

            Console.WriteLine("Monty Hall Problem Proof");
            Console.WriteLine($"Experimenting with {NUM_TRIALS} Trials");
            Console.WriteLine("Theoretical probability for winning without switching: 33.33%");
            Console.WriteLine("Theoretical probability for winning with switching: 66.66%");
            Console.WriteLine($"Probability of winning without switching: {(resultsWithoutSwitching.Count(x => x == 1) / (double)NUM_TRIALS) * 100:f2}%");
            Console.WriteLine($"Probability of winning with switching: {(resultsWithSwitching.Count(x => x == 1) / (double)NUM_TRIALS * 100):f2}%");
        }
    }
}