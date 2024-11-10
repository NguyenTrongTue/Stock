<template>
  <div class="home">


    <div class="home__right">
      <SearchStock />
    </div>
    <div class="home__center">
      <div class="home__center_balance">
        <div class="right">
          <div class="right_currency">{{ assets.total_net_assets }}</div>
          <div class="right_title">Hôm nay khoản đầu tư của Luân {{ assets.profit > 0 ? 'tăng' : 'giảm' }}</div>
          <div class="right_amount" :class="assets.profit > 0 ? 'green' : 'red'">{{ assets.profit > 0 ? '+' : '' }}{{
            assets.profit }}</div>
        </div>
        <div class="left">
          <micon type="Wallet" />
        </div>
      </div>
      <MLineChart />

    </div>
    <div class="home__left">
      <div class="home__left_title">Tin vĩ mô</div>

      <div class="list_news">
        <div class="list_news_item" v-for="(item, index) in news" :key="index">
          <div class="left">
            <div class="title">
              {{ item.title }}
            </div>
            <div class="time">10h trước</div>
          </div>
          <div class="right">
            <img :src="item.image" />

          </div>
        </div>

      </div>
    </div>
  </div>
</template>

<script>
import SideBar from "@/components/sidebar/SideBar.vue";
import MLineChart from '@/components/line-chart/MLineChart.vue';
import SearchStock from '@/components/search-stock/SearchStock.vue';

import StockAPI from '@/apis/StockAPI';
export default {
  components: {
    SideBar,
    MLineChart,
    SearchStock
  },
  data() {
    return {
      news: [
        {
          title: 'Được gì từ loạt động thái ‘hạ nhiệt’ tỷ giá?',
          image: 'https://image.tienphong.vn/600x315/Uploaded/2024/unvjthp/2024_06_27/u-5921.jpg'
        },
        {
          title: 'Được gì từ loạt động thái ‘hạ nhiệt’ tỷ giá?',
          image: 'https://image.tienphong.vn/600x315/Uploaded/2024/unvjthp/2024_06_27/u-5921.jpg'
        },
        {
          title: 'Được gì từ loạt động thái ‘hạ nhiệt’ tỷ giá?',
          image: 'https://image.tienphong.vn/600x315/Uploaded/2024/unvjthp/2024_06_27/u-5921.jpg'
        },
        {
          title: 'Được gì từ loạt động thái ‘hạ nhiệt’ tỷ giá?',
          image: 'https://image.tienphong.vn/600x315/Uploaded/2024/unvjthp/2024_06_27/u-5921.jpg'
        },
        {
          title: 'Được gì từ loạt động thái ‘hạ nhiệt’ tỷ giá?',
          image: 'https://image.tienphong.vn/600x315/Uploaded/2024/unvjthp/2024_06_27/u-5921.jpg'
        },
        {
          title: 'Được gì từ loạt động thái ‘hạ nhiệt’ tỷ giá?',
          image: 'https://image.tienphong.vn/600x315/Uploaded/2024/unvjthp/2024_06_27/u-5921.jpg'
        },
        {
          title: 'Được gì từ loạt động thái ‘hạ nhiệt’ tỷ giá?',
          image: 'https://image.tienphong.vn/600x315/Uploaded/2024/unvjthp/2024_06_27/u-5921.jpg'
        },
        {
          title: 'Được gì từ loạt động thái ‘hạ nhiệt’ tỷ giá?',
          image: 'https://image.tienphong.vn/600x315/Uploaded/2024/unvjthp/2024_06_27/u-5921.jpg'
        },
        {
          title: 'Được gì từ loạt động thái ‘hạ nhiệt’ tỷ giá?',
          image: 'https://image.tienphong.vn/600x315/Uploaded/2024/unvjthp/2024_06_27/u-5921.jpg'
        },
        {
          title: 'Được gì từ loạt động thái ‘hạ nhiệt’ tỷ giá?',
          image: 'https://image.tienphong.vn/600x315/Uploaded/2024/unvjthp/2024_06_27/u-5921.jpg'
        },
        {
          title: 'Được gì từ loạt động thái ‘hạ nhiệt’ tỷ giá?',
          image: 'https://image.tienphong.vn/600x315/Uploaded/2024/unvjthp/2024_06_27/u-5921.jpg'
        },
        {
          title: 'Được gì từ loạt động thái ‘hạ nhiệt’ tỷ giá?',
          image: 'https://image.tienphong.vn/600x315/Uploaded/2024/unvjthp/2024_06_27/u-5921.jpg'
        },
      ],
      assets: {
        total_net_assets: 0,
        profit: 0
      }
    };
  },

  mounted() {
    this.getAssetDashboard();

  },

  methods: {

    async getAssetDashboard() {
      const me = this;
      try {

        let user = me.$ms.cache.getCache("user");
        me.user = user;
        if (user) {
          me.$store.commit("showLoading")
          var result = await StockAPI.getAssetDashboard(user.user_id);
          me.$store.commit("hideLoading")
          if (result.total_net_assets) {

            me.assets = {
              total_net_assets: result.total_net_assets.toLocaleString('en-US'),
              profit: result.profit.toLocaleString('en-US')

            };
          }
        }
      }
      catch (error) {
        console.log(error);
        me.$store.commit("hideLoading")
      }
    },
  }
}
</script>

<style lang="scss">
@import "./home.scss";
</style>
