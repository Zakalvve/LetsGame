namespace LetsGame.Common.Data.Entities.Core;

public interface IIdable<TId> : IEntityMetaData
{
    TId Id { get; set; }
}