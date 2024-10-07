<template>
  <div class="asset">
    <div class="asset__left">
      <SearchStock />
    </div>
    <div class="asset__right">
      <div class="asset__right__title">Tổng quan tài sản</div>
      <div class="asset__right__content">
        <div class="doughnut mb-2">
          <div class="flex-between mb-2 gap-2">
            <div class="chart_title flex-between"><span>Tài sản ròng</span>
              <span>18.9 triệu</span>
            </div>
            <div>
              <mbutton @click="depositAmount" buttonText="Nạp tiền tài khoản" borderType="secondary" />
            </div>
          </div>
          <Doughnut />
        </div>
        <div class="column-chart">
          <div class="chart_title flex-between"><span>Lịch sử tài sản</span>
            <div class="choose_period">
              <div class="choose_period_item" :class="{ 'active': item.key == currentPeriod }" v-for="(item) in periods"
                :key="item.key" @click="currentPeriod = item.key">{{ item.label }}</div>

            </div>
          </div>
          <ColumnChart />
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import Doughnut from '@/components/doughnut/Doughnut.vue';
import ColumnChart from '@/components/column-chart/ColumnChart.vue';
import SearchStock from '@/components/search-stock/SearchStock.vue';
export default {
  components: {
    Doughnut,
    SearchStock,
    ColumnChart
  },
  data() {
    return {
      currentPeriod: '1W',
      periods: [
        {
          key: '1W',
          label: '1W',
        },
        {
          key: '1M',
          label: '1M',
        },
        {
          key: '6M',
          label: '6M',
        },
        {
          key: '1Y',
          label: '1Y',
        },
      ],
    };
  },

  async mounted() {

  },

  methods: {

    depositAmount() {

      this.$store.commit("showToast", {
        label: "Bạn đã nạp thành công 100 triệu vào tài khoản",
        type: 'success'
      });
    }
  },
};
</script>

<style lang="scss" scoped>
@import "./asset.scss";

.choose_period {
  display: flex;
  align-items: center;
  justify-content: flex-start;
  gap: 12px;

  .choose_period_item {
    box-shadow: rgba(0, 0, 0, 0.1) 0px 2px 2px;
    padding: 6px 12px;
    line-height: 1.125;
    height: 28px;
    background-color: rgba(51, 51, 51, 0.5);
    font-weight: 500;
    border-radius: 4px;
    cursor: pointer;

    &.active {
      background-color: rgb(212, 67, 67);
      color: rgb(228, 228, 236);
    }
  }
}
</style>
