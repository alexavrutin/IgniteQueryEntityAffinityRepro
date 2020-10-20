using Apache.Ignite.Core;
using Apache.Ignite.Core.Cache.Affinity;
using Apache.Ignite.Core.Cache.Configuration;
using Apache.Ignite.Core.Configuration;
using Apache.Ignite.Core.Transactions;
using System;
using System.Collections.Generic;

namespace IgniteQueryEntityAffinityRepro
{
    public class Program
    {
        static void Main(string[] args)
        {
            using (IIgnite ignite = Ignition.Start(GetConfiguration()))
            {
                var cache = ignite.GetCache<AffinityKey, BespokeCharge>("BespokeCharge");
                var bespokeCharge = new BespokeCharge()
                {
                    Id = 1,
                    PartnerId = 1,
                    ChargeValue = 1000,
                    ChargeValueIncCommission = 1100,
                    BespokeChargeTypeId = 1
                };
                cache.Put(new AffinityKey(bespokeCharge.Id, bespokeCharge.PartnerId), bespokeCharge);
                Console.ReadLine();
            }
        }

        public static IgniteConfiguration GetConfiguration()
        {
            // Configure Apache Ignite
            var igniteConfig = new IgniteConfiguration
            {
                CacheConfiguration = GetCacheConfiguration(),
                PeerAssemblyLoadingMode = Apache.Ignite.Core.Deployment.PeerAssemblyLoadingMode.CurrentAppDomain,
                JvmOptions = new List<string>() {
                    "-Djava.net.preferIPv4Stack=true",
                    "-DIGNITE_PERFORMANCE_SUGGESTIONS_DISABLED=false",
                    "-DIGNITE_QUIET=true",
                    "-DIGNITE_ALLOW_DML_INSIDE_TRANSACTION=true"
                },
                ClientConnectorConfiguration = new ClientConnectorConfiguration
                {
                    Port = 10801,
                    PortRange = 50
                },
                TransactionConfiguration = new TransactionConfiguration
                {
                    DefaultTransactionConcurrency = TransactionConcurrency.Pessimistic,
                    DefaultTransactionIsolation = TransactionIsolation.RepeatableRead
                }
            };

            return igniteConfig;
        }

        private static IEnumerable<CacheConfiguration> IterateCacheConfigurations()
        {
            // Data items
            yield return GetDefaultConfiguration<BespokeCharge>();
        }

        private static ICollection<CacheConfiguration> GetCacheConfiguration()
            => new List<CacheConfiguration>(IterateCacheConfigurations());

        private static CacheConfiguration GetDefaultConfiguration<TEntity>()
            where TEntity : InMemoryEntityBase, new()
        {
            return GetDefaultConfiguration<TEntity, int>();
        }

        private static CacheConfiguration GetDefaultConfiguration<TEntity, TKey>()
            where TEntity : InMemoryEntityBase, new()
        {
            var tableName = typeof(TEntity).Name;
            return new CacheConfiguration(tableName,
                new QueryEntity(typeof(AffinityKey), typeof(TEntity))
                {
                    KeyFieldName = "Id",
                    TableName = tableName
                })
            {
                SqlIndexMaxInlineSize = 1024,
                SqlSchema = "PUBLIC",
                AtomicityMode = CacheAtomicityMode.Transactional,
                CacheMode = CacheMode.Partitioned,
                Backups = 1
            };
        }
    }
}
