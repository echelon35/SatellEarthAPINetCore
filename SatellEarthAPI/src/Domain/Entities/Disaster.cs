using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SatellEarthAPI.Domain.Entities;
public class Disaster : BaseAuditableEntity
{
    #region Properties
    /// <summary>
    /// First time disaster was seen
    /// </summary>
    public DateTime PremierReleve { get; set; }

    /// <summary>
    /// Last time disaster was seen
    /// </summary>
    public DateTime DernierReleve { get; set; }

    /// <summary>
    /// Where the event came from
    /// </summary>
    public string? LienSource { get; set; }

    /// <summary>
    /// How much people have felt it
    /// </summary>
    public int NbRessenti { get; set; }

    /// <summary>
    /// Show disaster on website ?
    /// </summary>
    public Boolean Visible { get; set; }

    /// <summary>
    /// Alea type of disaster
    /// </summary>
    public Alea Alea { get; set; } = null!;

    #endregion Properties

}
