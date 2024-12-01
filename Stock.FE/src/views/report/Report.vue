<template>
  <div class="asset">
    <div class="asset__left">
      <SearchStock />
    </div>
    <div class="asset__right">
      <div class="asset__right__title">Báo cáo</div>

      <div class="asset__right__content">
        <div class="column-chart">
          <div class="chart_title flex-between"><span>Top 10 mã tăng nhiều nhất trong ngày</span></div>
          <ColumnChart :dataProps="desc" />
        </div>
        <div class="column-chart">
          <div class="chart_title flex-between"><span>Top 10 mã giảm nhiều nhất trong ngày</span></div>
          <ColumnChart :dataProps="asc" />
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import ColumnChart from './column-chart/ColumnChart.vue';
import SearchStock from '@/components/search-stock/SearchStock.vue';
import StockAPI from '@/apis/StockAPI';
export default {
  components: {
    SearchStock,
    ColumnChart
  },

  data() {
    return {
      desc: {},
      asc: {},
    };
  },


  mounted() {
    this.getReportStock();
  },

  methods: {
    async getReportStock() {
      const me = this;
      try {
        // Lấy user hiện tại đang login trong cache
        let user = this.$ms.cache.getCache("user");
        if (user) {
          me.$store.commit("showLoading");
          const result = await StockAPI.getReportStock();
          me.$store.commit("hideLoading");
          if (result.desc || result.asc) {
            me.desc = {
              data: result.desc.map(item => item.change_price),
              labels: result.desc.map(item => item.stock_code)
            }
            me.asc = {
              data: result.asc.map(item => item.change_price),
              labels: result.asc.map(item => item.stock_code)
            }
          }


        }
      } catch (error) {
        console.log(error);
        me.$store.commit("hideLoading");
      }
    }
  },
};
</script>

<style lang="scss" scoped>
@import "./report.scss";
</style>
