using System;

namespace Copious.Main
{
    public static class EnvironmentConfigurator
    {
        private const string Development = "Development";
        private const string Production = "Production";
        private const string Staging = "Staging";

        public static bool IsDevelopment(string env) => env.Equals(Development, StringComparison.OrdinalIgnoreCase);

        public static bool IsProduction(string env) => env.Equals(Production, StringComparison.OrdinalIgnoreCase);

        public static bool IsStaging(string env) => env.Equals(Staging, StringComparison.OrdinalIgnoreCase);

        public static void RunBasedOnEnvironment(string env, Action development, Action production, Action staging, Action none)
        {
            if (string.IsNullOrWhiteSpace(env))
            {
                none?.Invoke();
                return;
            }

            if (IsDevelopment(env))
            {
                development?.Invoke();
                return;
            }

            if (IsProduction(env))
            {
                production?.Invoke();
                return;
            }

            if (IsStaging(env))
            {
                staging?.Invoke();
                return;
            }
        }

        public static TReturn RunBasedOnEnvironment<TReturn>(string env, Func<TReturn> development, Func<TReturn> production, Func<TReturn> staging, Func<TReturn> none)
        {
            if (string.IsNullOrWhiteSpace(env))
            {
                return none == null ? default(TReturn) : none.Invoke();
            }

            if (IsDevelopment(env))
            {
                return development == null ? default(TReturn) : development.Invoke();
            }

            if (IsProduction(env))
            {
                return production == null ? default(TReturn) : production.Invoke();
            }

            if (IsStaging(env))
            {
                return staging == null ? default(TReturn) : staging.Invoke();
            }

            return default(TReturn);
        }
    }
}