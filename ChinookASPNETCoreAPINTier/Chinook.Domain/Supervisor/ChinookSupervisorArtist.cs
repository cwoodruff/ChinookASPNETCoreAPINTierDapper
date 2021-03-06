using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Chinook.Domain.Extensions;
using Chinook.Domain.ApiModels;

namespace Chinook.Domain.Supervisor
{
    public partial class ChinookSupervisor
    {
        public async Task<IEnumerable<ArtistApiModel>> GetAllArtistAsync(CancellationToken ct = default)
        {
            var artists = await _artistRepository.GetAllAsync(ct);
            return artists.ConvertAll();
        }

        public async Task<ArtistApiModel> GetArtistByIdAsync(int id, CancellationToken ct = default)
        {
            var artistViewModel = (await _artistRepository.GetByIdAsync(id, ct)).Convert;
            artistViewModel.Albums = (await GetAlbumByArtistIdAsync(artistViewModel.ArtistId, ct)).ToList();
            return artistViewModel;
        }

        public async Task<ArtistApiModel> AddArtistAsync(ArtistApiModel newArtistViewModel,
            CancellationToken ct = default)
        {
            /*var artist = new Artist
            {
                Name = newArtistViewModel.Name
            };*/

            var artist = newArtistViewModel.Convert;

            artist = await _artistRepository.AddAsync(artist, ct);
            newArtistViewModel.ArtistId = artist.ArtistId;
            return newArtistViewModel;
        }

        public async Task<bool> UpdateArtistAsync(ArtistApiModel artistViewModel,
            CancellationToken ct = default)
        {
            var artist = await _artistRepository.GetByIdAsync(artistViewModel.ArtistId, ct);

            if (artist == null) return false;
            artist.ArtistId = artistViewModel.ArtistId;
            artist.Name = artistViewModel.Name;

            return await _artistRepository.UpdateAsync(artist, ct);
        }

        public Task<bool> DeleteArtistAsync(int id, CancellationToken ct = default) 
            => _artistRepository.DeleteAsync(id, ct);
    }
}