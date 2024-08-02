using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MISA.AMIS.DEMO.Core;
using static Dapper.SqlMapper;

namespace MISA.AMIS.DEMO.API.Controllers
{
    public class BaseController<TCreateDto, TDto, TEntity> : ControllerBase where TEntity : class
    {
        #region Fields
        protected readonly IBaseService<TCreateDto, TDto, TEntity> _baseService;
        #endregion

        #region Contructor
        public BaseController(IBaseService<TCreateDto, TDto, TEntity> baseService)
        {
            _baseService = baseService;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Lấy danh sách đối tượng
        /// </summary>
        /// <returns>Trạng thái 200 && Danh sách đối tượng</returns>
        /// CreatedBy: QuangHuy (26/12/2023)
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await _baseService.GetAsync();
            return StatusCode(StatusCodes.Status200OK, result);
        }

        /// <summary>
        /// Lấy 1 đối tượng theo id
        /// </summary>
        /// <param name="id">Id đới tượng</param>
        /// <returns>Trạng thái 200 && Thông tin 1 đối tượng</returns>
        /// CreatedBy: QuangHuy (26/12/2023)
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var result = await _baseService.GetAsync(id);
            return StatusCode(StatusCodes.Status200OK, result);
        }

        /// <summary>
        /// Hàm tạo mới 1 đối tượng
        /// </summary>
        /// <param name="entity">Entity tương ứng</param>
        /// <returns>Trạng thái 201</returns>
        /// CreatedBy: QuangHuy (26/12/2023)
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] TCreateDto entity)
        {
            await _baseService.InsertAsync(entity);
            return StatusCode(StatusCodes.Status201Created);
        }

        /// <summary>
        /// Hàm cập nhật 1 đối tượng
        /// </summary>
        /// <param name="entity">Entity tương ứng</param>
        /// <param name="id">Id nhận qua query</param>
        /// <returns>Trạng thái 202</returns>
        /// CreatedBy: QuangHuy (26/12/2023)
        [HttpPut(template: "{id}")]
        public async Task<IActionResult> Update([FromBody] TCreateDto entity, Guid id)
        {
            await _baseService.UpdateAsync(id, entity);
            return StatusCode(statusCode: StatusCodes.Status202Accepted);
        }

        /// <summary>
        /// Hàm xóa 1 đối tượng
        /// </summary>
        /// <param name="id">Id nhận qua query</param>
        /// <returns></returns>
        /// CreatedBy: QuangHuy (26/12/2023)
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _baseService.DeleteAsync(id);
            return StatusCode(StatusCodes.Status204NoContent);
        }

        /// <summary>
        /// Hàm xóa 1 danh sách đối tượng theo Id
        /// </summary>
        /// <param name="ids">Danh sách Guid Id đối tượng</param>
        /// <returns>Status 204</returns>
        /// CreatedBy: QuangHuy (19/01/2024)
        [HttpDelete]
        public async Task<IActionResult> DeleteMutiple([FromBody] List<Guid> ids)
        {
            await _baseService.DeleteAsync(ids);
            return StatusCode(StatusCodes.Status204NoContent);
        }
        #endregion
    }
}
