using Mapster;
using MinhaBiblioteca.Application.DTOs;
using MinhaBiblioteca.Domain.Entities;

namespace MinhaBiblioteca.Application.Mappings;

public static class MapperExtensions
{
    public static TEntity MapearParaEntidade<TEntity, TDto>(this TDto dto)
        where TEntity : Entity
        where TDto : BaseDto<Guid>
    {
        return dto.Adapt<TEntity>();
    }

    public static TDto MapearParaDto<TEntity, TDto>(this TEntity entity)
        where TEntity : Entity
        where TDto : BaseDto<Guid>
    {
        return entity.Adapt<TDto>();
    }

    public static IEnumerable<TDto> MapearParaListaDto<TEntity, TDto>(this IEnumerable<TEntity> entities)
        where TEntity : Entity
        where TDto : BaseDto<Guid>
    {
        return entities.Adapt<IEnumerable<TDto>>();
    }

    public static IEnumerable<TEntity> MapearParaListaEntidade<TDto, TEntity>(this IEnumerable<TDto> dtos)
        where TEntity : Entity
        where TDto : BaseDto<Guid>
    {
        return dtos.Adapt<IEnumerable<TEntity>>();
    }
}
