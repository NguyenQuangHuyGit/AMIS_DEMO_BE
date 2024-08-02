using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.DEMO.Core
{
    /// <summary>
    /// Service dùng chung cho các đối tượng
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class BaseService<TCreateDto, TDto, TEntity> : IBaseService<TCreateDto, TDto, TEntity> where TEntity : class, IGetPrimaryKey
    {
        #region Fields
        protected readonly IBaseRepository<TEntity> _baseRepository;
        protected readonly IMapper _mapper;
        #endregion

        #region Contructor
        public BaseService(IBaseRepository<TEntity> baseRepository, IMapper mapper)
        {
            _baseRepository = baseRepository;
            _mapper = mapper;
        }
        #endregion

        #region Methods
        public virtual async Task<IEnumerable<TDto>> GetAsync()
        {
            var result = await _baseRepository.GetAsync();
            var resultDto = _mapper.Map<IEnumerable<TDto>>(result);
            return resultDto;
        }

        public virtual async Task<TDto?> GetAsync(Guid id)
        {
            var result = await _baseRepository.GetAsync(id);
            var resultDto = _mapper.Map<TDto?>(result);
            return resultDto;
        }

        public virtual async Task InsertAsync(TCreateDto createDto)
        {
            await ValidateCreateBusiness(createDto);
            var entity = _mapper.Map<TEntity>(createDto);
            var propertyKey = entity.GetKeyName();
            if (propertyKey != null)
            {
                typeof(TEntity).GetProperty(propertyKey)?.SetValue(entity, Guid.NewGuid());
            }
            if(entity is BaseEntity baseEntity)
            {
                baseEntity.CreatedBy = "QuangHuy";
                baseEntity.CreatedDate = DateTime.Now;
            }
            await _baseRepository.InsertAsync(entity);
        }

        public virtual async Task UpdateAsync(Guid id, TCreateDto createDto)
        {
            await ValidateUpdateBusiness(id,createDto);
            var entity = _mapper.Map<TEntity>(createDto);
            if (entity is BaseEntity baseEntity)
            {
                baseEntity.ModifiedBy = "QuangHuy";
                baseEntity.ModifiedDate = DateTime.Now;
            }
            await _baseRepository.UpdateAsync(id, entity);
        }

        public virtual async Task DeleteAsync(Guid id)
        {
            await _baseRepository.DeleteAsync(id);
        }

        public virtual async Task DeleteAsync(List<Guid> ids)
        {
            await _baseRepository.DeleteAsync(ids);
        }

        /// <summary>
        /// Hàm check trùng 1 trường của một đối tượng bất kỳ
        /// </summary>
        /// <param name="value">Giá trị trường muốn kiểm tra của đối tượng</param>
        /// <returns>False: Có trùng || True: Không trùng lặp</returns>
        /// CreatedBy: QuangHuy (08/01/2024)
        protected async virtual Task<bool> CheckDuplicateFieldAsync(string field, string value)
        {
            var result = await _baseRepository.FindByFieldAsync(field, value);
            if (result != null)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Hàm xử lý kiểm tra tính hợp lệ của dữ liệu trước khi tiến hành thêm mới 1 đối tượng
        /// </summary>
        /// <param name="dto">DTO hứng đối tượng thêm mới</param>
        /// <returns></returns>
        /// CreatedBy: QuangHuy (08/01/2024)
        protected virtual Task ValidateCreateBusiness(TCreateDto dto)
        {
            return Task.CompletedTask;
        }

        /// <summary>
        /// Hàm xử lý kiểm tra tính hợp lệ của dữ liệu trước khi tiến hành cập nhật 1 đối tượng
        /// </summary>
        /// <param name="dto">DTO hứng đối tượng cập nhật</param>
        /// <param name="id">Guid Id đối tượng cập nhật</param>
        /// <returns></returns>
        /// CreatedBy: QuangHuy (08/01/2024)
        protected virtual Task ValidateUpdateBusiness(Guid id, TCreateDto dto)
        {
            return Task.CompletedTask;
        }

        #endregion
    }
}
