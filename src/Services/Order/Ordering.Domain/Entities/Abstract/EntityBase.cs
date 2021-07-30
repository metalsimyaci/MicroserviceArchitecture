using System.ComponentModel.DataAnnotations.Schema;

namespace Ordering.Domain.Entities.Abstract
{
	public abstract class EntityBase:IEntity
	{
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public virtual int Id { get; protected set; }

		public EntityBase Clone()
		{
			return (EntityBase) this.MemberwiseClone();
		}
	}
}
