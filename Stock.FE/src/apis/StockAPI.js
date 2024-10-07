import request from "@/utils/request.js";
import BaseAPI from "./BaseAPI";

class StockAPI extends BaseAPI {
  constructor() {
    super("stock");
  } 

  register(payload) {
    return request.post(this.url + "/register", payload);
  }

  login(payload) {
    return request.post(this.url + "/login", payload);
  }

  getStockByPeriod(payload) {
    return request.get(this.url + `/get_stock_by_period?stockId=${payload.stockId}&periodEnum=${payload.periodEnum}`);
  }
   getPopularStock() {
    return request.get(this.url + "/get_popular_stock");
  }
}

const instance = new StockAPI();

export default instance;
