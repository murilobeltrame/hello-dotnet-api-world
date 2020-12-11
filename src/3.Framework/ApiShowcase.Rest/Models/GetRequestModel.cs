namespace ApiShowcase.Rest.Models
{
    public class GetRequestModel
    {
        /// <summary>
        /// Gets or sets de offset of the query. If isnt specified, value 0 is assumed.
        /// </summary>
        /// <example>10</example>
        public int? _offset { get; set; }

        /// <summary>
        /// Gets or sets the limit of the resultset record count. If isnt`t specified, value 10 is assumed.
        /// </summary>
        public short? _limit { get; set; }

        /// <summary>
        /// Gets or sets the order for the resultset.
        /// Use the field name as sort argument. If a " DESC" sufix is defined, the sort order will be inverted.
        /// If isn`t specified, 'BusinessId' will be assumed.
        /// </summary>
        public string _order { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string _fields { get; set; }
    }
}