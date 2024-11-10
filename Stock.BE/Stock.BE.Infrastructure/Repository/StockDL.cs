using Dapper;
using Newtonsoft.Json;
using Stock.BE.Core;
using Stock.BE.Core.DL;
using Stock.BE.Core.Entity;
using Stock.BE.Core.Enum;
using Stock.BE.Core.Model;
using Stock.BE.Infrastructure.Repository.Base;
using System.Data;
using static Dapper.SqlMapper;

namespace Stock.BE.Infrastructure.Repository
{
    public class StockDL : BaseRepository<StockEntity>, IStockDL
    {
        public StockDL(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
        /// <summary>
        /// Đầu lấy dữ liệu sự thay đổi của các mã theo từng khoảng thời gain
        /// </summary>
        /// <param name="stockId">ID của chứng khoán</param>
        /// <param name="periodEnum">Kỳ thay đổi</param>
        /// <returns></returns>
        public async Task<List<object>> GetStockByPeriodAsync(Guid stockId, PeriodEnum periodEnum)
        {
            var paramDictionary = new Dictionary<string, object>()
                {
                    {"p_stock_id", stockId }
                };
            var param = new DynamicParameters(paramDictionary);

            // Xác định khoảng thời gian dựa trên periodEnum
            string interval = periodEnum switch
            {
                PeriodEnum.Week1 => "1 week",
                PeriodEnum.Month1 => "1 month",
                PeriodEnum.Month6 => "6 months",
                _ => "1 year" // Mặc định là 1 năm nếu không thuộc các trường hợp trên
            };
            var sql = $@"SELECT distinct 
                                                stock_id,
                                                DATE(created_at) as created_at,
                                                avg(current_price) over  (partition by DATE(created_at)) as current_price, 
                                                avg(change_price) over  (partition by DATE(created_at)) as change_price, 
                                                avg(change_price_by_percent) over  (partition by DATE(created_at)) as change_price_by_percent 
                                        FROM stock_price_changes 
                                        WHERE stock_id =@p_stock_id AND created_at BETWEEN NOW() - INTERVAL '{interval}' AND NOW() order by DATE(created_at) desc;";
            var result = await _uow.Connection.QueryAsync<object>(sql, param, commandType: CommandType.Text);
            return result.ToList();
        }
        /// <summary>
        /// Lấy lên các chứng khoán phổ biến
        /// </summary>
        /// <returns></returns>
        public async Task<List<StockEntity>> GetPopularStockAsync()
        {
            var sql = @"select * from stocks where stock_id in ('ac29582d-e41a-4576-9596-4ac398f14f84',
                                            '9db887ad-1c70-479e-87c6-4bdcabf71d4e',
                                            '75f14796-133c-4691-aa3d-e31a7f290c91')";
            var result = await _uow.Connection.QueryAsync<StockEntity>(sql);
            return result.ToList();
        }
        /// <summary>
        /// Lấy tài sản và sự thay đổi tài sản của người dùng theo ngày
        /// </summary>
        /// <param name="userId">Id của người dùng</param>
        /// <returns></returns>
        public async Task<AssetModel> GetAssetByUserAsync(Guid userId)
        {
            var param = new DynamicParameters();
            param.Add("@userId", userId);

            var sql = @"select total_net_assets from ""user"" u where user_id = @userId; 
                                    select sum(profit_loss) from deals d where user_id = @userId;";

            using (var multi = await _uow.Connection.QueryMultipleAsync(sql, param))
            {
                var data1 = await multi.ReadAsync<double>();
                var data2 = await multi.ReadAsync<double>();

                var result = new AssetModel
                {
                    total_asset = data1.FirstOrDefault(),
                    change_asset = data2.FirstOrDefault()
                };

                return result;
            }
        }
        /// <summary>
        /// Đầu lấy ra thông tin các deal nắm giữ của người dùng
        /// </summary>
        /// <param name="userId">Id của người dùng</param>
        /// <returns></returns>
        public async Task<object> GetDealsByUserAsync(Guid userId)
        {
            var param = new DynamicParameters();
            param.Add("@userId", userId);

            var sql = @"select 
	                d.deal_id,
	                d.stock_id,
	                d.stock_code,
	                d.total_volume, 
	                d.total_tradeable_volume,
	                d.matched_price, 
	                s.matched_price as current_price,
	                d.matched_price * d.total_volume * 1000 as cost_value ,
	                (s.matched_price - d.matched_price) * d.total_volume * 1000 as profit_loss, 
	                ((s.matched_price - d.matched_price) * d.total_volume * 1000)
		                /
	                (d.matched_price * d.total_volume * 1000 ) * 100 as profit_loss_by_percent,
	                s.difference,
	                case 
		                when s.matched_price - d.matched_price > 0 then 0
	                else 1 end as is_profit
                from 
	                deals d 
                join stocks s on s.stock_id = d.stock_id
                where d.user_id = @userId;";
            var result = await _uow.QueryList<object>(sql, param);
            return result;
        }
        /// <summary>
        /// Đầu lấy các giao dịch của người dùng
        /// </summary>
        /// <param name="userId">Id của người dùng</param>
        /// <returns></returns>
        public async Task<object> GetTransactionsByUserAsync(Guid userId)
        {
            var param = new DynamicParameters();
            param.Add("@userId", userId);
            var sql = @"select * from transactions d where user_id = @userId;";
            var result = await _uow.QueryList<object>(sql, param);
            return result;
        }
        /// <summary>
        /// Đầu lấy thông tin của người dùng bao gồm luôn cả tài sản
        /// </summary>
        /// <param name="userId">Id của người dùng</param>
        /// <returns></returns>
        public async Task<object> GetUserByIdAsync(Guid userId)
        {
            var param = new DynamicParameters();
            param.Add("@userId", userId);
            var sql = @"select * from user d where user_id = @userId;";
            var result = await _uow.QueryList<object>(sql, param);
            return result;
        }
        /// <summary>
        /// Đầu thêm tiền mặt cho người dùng
        /// </summary>
        /// <param name="userId">Id của người dùng</param>
        /// <param name="amount"></param>
        /// <returns></returns>
        public async Task AddCashByUser(Guid userId)
        {
            var paramDictionary = new Dictionary<string, object>()
            {
                {"p_user_id", userId }
            };
            var param = new DynamicParameters(paramDictionary);
            var sql = "select * from public.update_cash_value_by_user(@p_user_id)";
            await _uow.Connection.ExecuteAsync(sql, param, commandType: CommandType.Text);

        }
        /// <summary>
        /// Đầu thêm các deal nắm giữ của người dùng
        /// </summary>
        /// <returns></returns>
        public async Task InsertDeal(DealDTO dealDTO)
        {
            var deal = new DealEntity
            {
                deal_id = Guid.NewGuid(),
                stock_id = dealDTO.stock_id,
                stock_code = dealDTO.stock_code,
                total_volume = dealDTO.total_volume,
                total_tradeable_volume = dealDTO.total_tradeable_volume,
                market_value = dealDTO.market_value,
                matched_price = dealDTO.matched_price,
                current_price = dealDTO.current_price,
                cost_value = dealDTO.cost_value,
                profit_loss = dealDTO.profit_loss,
                profit_loss_by_percent = dealDTO.profit_loss_by_percent
            };
            await base.BaseInsertAsync(deal, "deals");
        }
        /// <summary>
        /// Đầu thêm các giao dịch mua bán của người dùng
        /// </summary>
        /// <returns></returns>
        public async Task<AddTransactionResult> InsertTransaction(TransactionsDTO transactionsDto)
        {
            var result = new AddTransactionResult();
            var transaction = new TransactionsEntity
            {
                transactions_id = Guid.NewGuid(),
                stock_id = transactionsDto.stock_id,
                user_id = transactionsDto.user_id,
                stock_code = transactionsDto.stock_code,
                transaction_type = transactionsDto.transaction_type,
                created_at = DateTime.Now,
                order_price = transactionsDto.order_price,
                matched_price = transactionsDto.matched_price,
                volume = transactionsDto.volume,
            };

            string transactionJson = JsonConvert.SerializeObject(transaction);

            var param = new Dictionary<string, object>
                {
                    { $"@p_transactions", transactionJson }
                };
            var parameters = new DynamicParameters(param);
            if (transaction.transaction_type == 0)
            {
                var sql = "select * from public.buy_stock(@p_transactions::jsonb);";
                await _uow.Connection.ExecuteAsync(sql, parameters);
            }
            else
            {
                var obj = await this.GetTotalVolumeByUserId(transaction.user_id);
                if (transaction.volume > obj?.total_volume)
                {
                    result.Success = false;
                    result.Message = "Số lượng bán không được phép lớn hơn số lượng sở hữu hiện có ";
                }
                parameters.Add("@p_sell_full", transaction.volume == obj?.total_volume);
                var sql = "select * from public.sell_stock(@p_transactions::jsonb, @p_sell_full);";
                await _uow.Connection.ExecuteAsync(sql, parameters);
            }
            return result;
        }

        private async Task<DealEntity?> GetTotalVolumeByUserId(Guid userId)
        {
            var param = new Dictionary<string, object>
             {
                    { $"@userId", userId }
             };

            var sql = @"select sum(total_volume) as total_volume from deals d where d.user_id  = @userId;";
            var entity = await _uow.QueryDefault<DealEntity>(sql, param);
            return entity;
        }

        public async Task UpdateStockPriceChange()
        {
            //var sql = "select * from public.calculate_stock_change();";
            var sql = "select * from public.buy_sell_stock();";
            await _uow.Connection.ExecuteAsync(sql);
        }

        public async Task<List<TableAssetHistoryModel>> GetAssetHistoryByUserAsync(Guid userId, PeriodEnum periodEnum)
        {
            var paramDictionary = new Dictionary<string, object>()
                {
                    {"p_user_id", userId }
                };
            var param = new DynamicParameters(paramDictionary);

            // Xác định khoảng thời gian dựa trên periodEnum
            string interval = periodEnum switch
            {
                PeriodEnum.Week1 => "1 week",
                PeriodEnum.Month1 => "1 month",
                PeriodEnum.Month6 => "6 months",
                _ => "1 year" // Mặc định là 1 năm nếu không thuộc các trường hợp trên
            };

            // Câu truy vấn SQL tổng quát với khoảng thời gian động
            var sql = @$"
                    SELECT distinct 
                        user_id, 
                        DATE(created_at) as created_at,
                        avg(total_net_assets) OVER (PARTITION BY DATE(created_at)) as total_net_assets,
                        avg(stock_value) OVER (PARTITION BY DATE(created_at)) as stock_value,
                        avg(cash_value) OVER (PARTITION BY DATE(created_at)) as cash_value
                    FROM table_asset_history 
                    WHERE user_id = @p_user_id 
                    AND created_at BETWEEN NOW() - INTERVAL '{interval}' AND NOW();";

            var result = await _uow.Connection.QueryAsync<TableAssetHistoryModel>(sql, param, commandType: CommandType.Text);

            return result.ToList();
        }


        public async Task BuySellStockAsync()
        {
            var sql = "select * from public.buy_sell_stock();";
            await _uow.Connection.ExecuteAsync(sql);
        }

        public override async Task<StockEntity?> GetByIdAsync(Guid id)
        {
            var param = new Dictionary<string, object>
                {
                    { $"@stock_id", id }
                };

            var sql = $"select * from public.stocks where stock_id = @stock_id";

            var entity = await _uow.QueryDefault<StockEntity>(sql, param);

            return entity;
        }

        public async Task<object?> GetAssetDashboard(Guid userId)
        {
            var param = new Dictionary<string, object>
                {
                    { $"@userId", userId }
                };

            var sql = @"select 
	                                            sum((s.matched_price - d.matched_price) * d.total_volume * 1000) 
	                                            over (partition by d.user_id) 	as profit,
	                                            u.total_net_assets	
                                            from deals d 
                                            inner join stocks s on s.stock_id = d.stock_id 
                                            inner join ""user"" u on u.user_id = d.user_id 
                                            where u.user_id = @userId;";
            var result = await _uow.QueryDefault<object>(sql, param);

            return result;
        }
    }
}
