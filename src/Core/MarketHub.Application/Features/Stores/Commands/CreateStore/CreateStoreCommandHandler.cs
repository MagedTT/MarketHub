using AutoMapper;
using MarketHub.Application.Contracts.Persistence;
using MarketHub.Application.Responses;
using MarketHub.Domain.Entities;
using MediatR;

namespace MarketHub.Application.Features.Stores.Commands.CreateStore;

public class CreateStoreCommandHandler : IRequestHandler<CreateStoreCommand, BaseResponse>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryManager _repositoryManager;
    public CreateStoreCommandHandler(IMapper mapper, IRepositoryManager repositoryManager)
    {
        _mapper = mapper;
        _repositoryManager = repositoryManager;
    }

    public async Task<BaseResponse> Handle(CreateStoreCommand request, CancellationToken cancellationToken)
    {
        BaseResponse response = new();

        Store store = _mapper.Map<Store>(request);

        _repositoryManager.StoreRepository.CreateStore(store);

        await _repositoryManager.SaveAsync();

        return response;
    }
}