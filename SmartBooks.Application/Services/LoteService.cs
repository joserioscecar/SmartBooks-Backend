using AutoMapper;
using SmartBooks.Application.DTOs.Lotes;
using SmartBooks.Application.Interfaces;
using SmartBooks.Domain.Entities;
using SmartBooks.Domain.Exceptions;
using SmartBooks.Domain.Interfaces;

namespace SmartBooks.Application.Services;

public class LoteService : ILoteService
{

    private readonly ILoteRepository _repo;
    private readonly IMapper _mapper;

    public LoteService(ILoteRepository repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public async Task AddAsync(string lote)
    {

        var loteExistente = await _repo.ExisteLoteAsync(lote);

        if (loteExistente)
        {
            throw new RuleBusinessException("El lote ya existe.");
        }

        var nuevoLote = new Lote
        {
            Codigo = lote,

        };

        await _repo.AddAsync(nuevoLote);
    
    }

    public async Task<IEnumerable<LoteDto>> GetLotesAsync()
    {

        var lotes = await _repo.GetLotesAsync();


        return _mapper.Map<IEnumerable<LoteDto>>(lotes);

    }


}
