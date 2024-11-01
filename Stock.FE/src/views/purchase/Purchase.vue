<template>
  <div class="purchase">
    <div class="purchase__left">
      <div class="tab">
        <div class="tab_header">
          <div class="tab_item" :class="{ 'active': item.tabKey == currentTab }" v-for="item in listTabs"
            :key="item.tabKey" @click="currentTab = item.tabKey">
            {{ item.title }}
          </div>

        </div>
        <div class="tab_content">
          <ctable v-if="currentTab == 0" :columns="columnsConfigs.normalCommandColumns"
            :datax="columnsConfigs.normalCommandDatax" />
          <ctable v-else :columns="columnsConfigs.dealColumns" :datax="columnsConfigs.dealDatax"
            @clickRow="handleChooseDeal" />
        </div>
      </div>
    </div>
    <div class="purchase__right">
      <div class="tab">
        <div class="tab_header">

          <div class="tab_item active">Lệnh thường</div>

        </div>
        <div class="tab_content">
          <div class="stock_wrapper">
            <div class="stock_search">

              <msearch v-model="search" :placeholder="'Tìm kiếm mã'" @onInput="handleSearchStock"></msearch>
              <div class="search_wrapper" v-if="showSearchResult">


                <div class="search_item" @click="handleChooseSearch(item)" v-for="(item, index) in searchResult"
                  :key="index">

                  <div class="search_item_top flex">
                    <div class="search_stock_code">{{ item.stock_code }}</div>
                    <div class="search_stock_price"
                      :class="{ 'green': item.difference === 0, 'red': item.difference === 1, 'yellow': item.difference === 2 }">
                      <span>
                        {{ item.matched_price }}
                      </span>
                      <span> ({{ item.percent }})</span>
                    </div>

                  </div>
                  <div class="search_item_bottom flex">
                    <div class="search_stock_flatform">{{ item.trading_floor }}</div>
                    <div class="search_stock_total_volume">{{ item.tradable_volume }}CP</div>
                  </div>
                </div>
              </div>
            </div>
            <div class="stock">
              <div class="stock_top">
                <div class="stock_top_right">
                  <div class="stock_code">
                    <span class="code">{{ currentStock.stock_code }}</span>
                    <span class="flatform">{{ currentStock.trading_floor }}</span>
                  </div>
                  <div class="stock_name">{{ currentStock.stock_name }}</div>
                </div>
                <div class="stock_top_left"
                  :class="{ 'green': currentStock.difference === 0, 'red': currentStock.difference === 1, 'yellow': currentStock.difference === 2 }">
                  <span class="matching_price">{{ currentStock.matched_price }}
                  </span>
                  <span class="price_changes">{{ currentStock.change_price }} </span>&nbsp;
                  <span class="price_percent">({{ currentStock.percent }}) </span>
                </div>
              </div>
              <div class="stock_bottom">
                <div class="price_item">
                  <div class="price_item_title">Sàn</div>
                  <div class="price_item_value blue">{{ currentStock.min_price }}</div>
                </div>
                <div class="price_item">
                  <div class="price_item_title">Tham chiếu</div>
                  <div class="price_item_value yellow">{{ currentStock.reference_price }}</div>
                </div>
                <div class="price_item">
                  <div class="price_item_title">Trần</div>
                  <div class="price_item_value purple">{{ currentStock.max_price }}</div>
                </div>
              </div>

            </div>
            <div class="purchasing_ability">
              <div class="purchasing_ability_title">Sức mua</div>
              <div class="purchasing_ability_value">{{ asset.cash_value }}</div>
            </div>

            <div class="set_price">
              <div class="set_price_title">Giá đặt</div>
              <minput :isAdjustable="true" name="order_price" v-model="objectMaster.order_price" ref="order_price"
                :allowDecimal="true" :defaultChangeValue="0.15" :checkError="true" class="set_price_input row-100"
                @onInput="handleSetPrice" />

            </div>

            <div class="set_price">
              <div class="set_price_title">KL đặt</div>
              <minput name="volume" :isAdjustable="true" v-model="objectMaster.volume" ref="volume" :allowNumber="true"
                :defaultChangeValue="100" :checkError="true" class="set_price_input row-100"
                @onInput="handleSetVolume" />

            </div>
          </div>
          <div class="purchase_action">
            <div class="buy" @click="handlePurchase">
              <div class="title">MUA</div>
              <div>{{ objectMaster.buying_ability }}</div>
            </div>
            <div class="sell" @click="handleSell">
              <div class="title">BÁN</div>
              <div>{{ objectMaster.purchasing_ability }}</div>
            </div>
          </div>
        </div>
      </div>

    </div>
  </div>
</template>

<script>
import * as config from './columnsConfig';
import StockAPI from '@/apis/StockAPI';
export default {
  data() {
    return {
      objectMaster: {
        buying_ability: 0,
        purchasing_ability: 0,
        order_price: 0,
        volume: 0
      },
      dataSearchs: [
      ],
      listTabs: [
        {
          tabKey: 0,
          title: 'Sổ lệnh thường',
        },
        {
          tabKey: 1,
          title: 'Deal nắm giữ',
        },
      ],
      currentTab: 0,
      columnsConfigs: config,
      currentStock: {},
      showSearchResult: false,
      search: "",
      searchResult: [],
      asset: {},
      user: {},
    };
  },

  async mounted() {
    const me = this;
    await me.handleGetDataSearch();
    me.getAssets();
  },


  watch: {
    'objectMaster.order_price'(price) {
      const me = this;
      if (me.currentStock && (price < me.currentStock.min_price || price > me.currentStock.max_price)) {
        me.$refs.order_price.setError('Giá ngoài biên độ');
      } else {
        me.$refs.order_price.setError('');
      }
      if (me.user && me.user.cash_value) {
        me.objectMaster.buying_ability = (me.user.cash_value / me.currentStock.reference_price / 1000).toFixed(0);
      }
    },
    'objectMaster.volume'(volume) {
      const me = this;
      console.log(volume);
      if (volume <= 0) {
        me.$refs.volume.setError('Khối lượng đặt phải lớn hơn 0.');
      } else if (volume > me.objectMaster.buying_ability) {
        me.$refs.volume.setError('Khối lượng đặt vượt quá số tiền hiện có.');
      } else {
        me.$refs.volume.setError('');
      }

    }

  },
  methods: {
    /**
     * Asynchronously handles the purchase operation.
     * - Validates that the volume is greater than 0 before proceeding.
     * - Constructs a payload with the stock information and user's details.
     * - Attempts to make a purchase request via the StockAPI.
     * - Displays a success toast message if the purchase is successful.
     * - Catches and logs any errors, displaying an error toast message if the purchase fails.
     */
    async handlePurchase() {
      const me = this;
      try {
        if (me.objectMaster.volume == 0) {
          this.$store.commit("showToast", {
            label: "Khối lượng đặt phải lớn hơn 0.",
            type: 'error'
          });
        }

        const payload = {
          stock_id: me.currentStock.stock_id,
          user_id: me.user.user_id,
          stock_code: me.currentStock.stock_code,
          transaction_type: 1,
          order_price: me.objectMaster.order_price,
          matched_price: me.currentStock.matched_price,
          volume: me.objectMaster.volume,
          status: 3,
          modified_at: new Date()
        }

        var result = await StockAPI.purchase(payload);

        if (result) {
          this.$store.commit("showToast", {
            label: "Đặt lệnh mua thành công",
            type: 'success'
          });
        }
      } catch (error) {
        console.error(error);
        this.$store.commit("showToast", {
          label: "Đặt lệnh mua thất bại",
          type: 'error'
        });

      }
    }
    ,
    handleSell() { }
    ,
    handleSearchStock(key) {
      const me = this;
      me.showSearchResult = true;
      if (key) {
        me.searchResult = me.dataSearchs.filter(item => {
          return item.stock_code.includes(key);
        })
      } else {
        me.searchResult = me.dataSearchs.map(item => {
          return {
            ...item
          }
        })
      }
    },
    handleChooseSearch(stock) {
      const me = this;
      me.currentStock = stock;
      me.showSearchResult = false;
      me.search = stock.stock_code;
      me.objectMaster.order_price = stock.reference_price;
      let user = me.$ms.cache.getCache("user");
      if (user) {
        me.objectMaster.buying_ability = (user.cash_value / stock.reference_price / 1000).toFixed(0);
      }
    },
    handleChooseDeal(deal) {
      const me = this;
      me.objectMaster.buying_ability = deal.total_tradeable_volume;
      me.objectMaster.purchasing_ability = Math.floor(me.asset.cash / deal.current_price).toLocaleString('en-US');
    },
    async handleGetDataSearch() {
      const me = this;
      let data = await StockAPI.get();
      me.dataSearchs = data.map(stock => {
        return {
          ...stock,
          percent: `${stock.difference === 1 ? '-' :
            stock.difference === 0 ? '+' :
              ''}${((stock.change_price / stock.matched_price) * 100).toFixed(2)}%`
        }
      });
      me.searchResult = me.dataSearchs.map(item => {
        return {
          ...item
        }
      });
      me.search = me.dataSearchs[0].stock_code;
      me.currentStock = me.dataSearchs[0];

      me.objectMaster.order_price = me.currentStock.reference_price;
      let user = me.$ms.cache.getCache("user");
      if (user) {
        me.objectMaster.buying_ability = (user.cash_value / me.currentStock.reference_price / 1000).toFixed(0);
      }
    },
    getAssets() {
      const me = this;
      let user = me.$ms.cache.getCache("user");
      me.user = user;
      if (user) {
        me.asset = {
          cash_value: this.$ms.common.formatAmount(user.cash_value)
        }
      }

    }

  },
};
</script>

<style lang="scss">
@import "./purchase.scss";
</style>
