using Apache.Ignite.Core.Cache.Affinity;
using Apache.Ignite.Core.Cache.Configuration;
using System;

namespace IgniteQueryEntityAffinityRepro
{
    /// <summary>
    /// Represents a bespoke charge.
    /// </summary>
    [Serializable]
    public class BespokeCharge : InMemoryEntityBase
    {
        /// <summary>
        /// Internal bespoke charge Id. This numeric value encodes a UtilityClick Portal Guid.
        /// </summary>
        [QuerySqlField(IsIndexed = true)]
        public override int Id { get; set; }

        /// <summary>
        /// Bespoke charge type id. This numeric value encodes a UtilityClick Portal Guid.
        /// </summary>
        [QuerySqlField]
        public int BespokeChargeTypeId { get; set; }

        /// <summary>
        /// Bespoke charge value.
        /// </summary>
        [QuerySqlField]
        public decimal ChargeValue { get; set; }

        /// <summary>
        /// Bespoke charge value with commission.
        /// </summary>
        [QuerySqlField]
        public decimal ChargeValueIncCommission { get; set; }

        /// <summary>
        /// Reference to a QuoteRecord to which the Bespoke Charge beints.
        /// </summary>
        [QuerySqlField(IsIndexed = true)]
        public int QuoteRecordId { get; set; }

        /// <summary>
        /// Partner Id. This numeric value encodes a UtilityClick Portal Guid. Acts as an affinity key. 
        /// </summary>        
        [QuerySqlField]
        [AffinityKeyMapped]
        public override int PartnerId { get; set; }
    }

}
