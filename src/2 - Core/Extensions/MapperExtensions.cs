using Mapster;
using MinhaBiblioteca.Core.DTOs;
using MinhaBiblioteca.Core.Entities;

namespace MinhaBiblioteca.Core.Extensions;

public static class MapperExtensions
{
    public static TEntity MapearParaEntidade<TEntity, TDto>(this TDto dto)
        where TEntity : Entity
        where TDto : BaseDto
    {
        return dto.Adapt<TEntity>();
    }

    public static TDto MapearParaDto<TEntity, TDto>(this TEntity entity)
        where TEntity : Entity
        where TDto : BaseDto
    {
        return entity.Adapt<TDto>();
    }

    public static IEnumerable<TDto> MapearParaListaDto<TEntity, TDto>(this IEnumerable<TEntity> entities)
        where TEntity : Entity
        where TDto : BaseDto
    {
        return entities.Adapt<IEnumerable<TDto>>();
    }

    public static IEnumerable<TEntity> MapearParaListaEntidade<TDto, TEntity>(this IEnumerable<TDto> dtos)
        where TEntity : Entity
        where TDto : BaseDto
    {
        return dtos.Adapt<IEnumerable<TEntity>>();
    }
}
