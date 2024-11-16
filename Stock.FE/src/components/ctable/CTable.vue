<template>
  <div class="table_container">
    <table class="table">
      <thead>
        <tr>
          <th v-for="(column, index) in columns" :key="index" :style="{
            'text-align': column.textAlign,
            width: column.width ? column.width + 'px' : null,
            minWidth: column.width ? column.width + 'px' : null,
          }" :ref="column.field">
            <div class="th__content">
              <span>{{ column.headerName }}</span>
            </div>
          </th>
        </tr>
      </thead>
      <tbody class="tbody">
        <tr v-for="(item, index) in datax" :key="index">
          <td v-for="(column, index) in columns" :key="index" :style="{
            'text-align': column.textAlign,
            width: column.width ? column.width + 'px' : null,
            minWidth: column.width ? column.width + 'px' : null,
          }" :ref="column.field" @click="handleClickRow(item)">
            <div
              :class="`${column.functionCustomColorTD ? 'td__content_wrapper ' + column.functionCustomColorTD(item[column.field]) : ''}`">
              <span
                :class="`td__content ${column.funcCusColor ? column.funcCusColor(item[column.field], item[column.keyColor]) : ''}`">
                {{
                  formatData(column,
                    item[column.field], item.difference)
                }}</span>
            </div>
          </td>
        </tr>
      </tbody>
    </table>
    <div class="no-data" v-if="hasNoData">
      <img src="@/assets/img/no-data.png" alt="">
      <span class="no-data-text">{{ noDataText }}</span>

    </div>

  </div>
</template>

<script>
import Enums from '@/resources/enumeration';
export default {
  name: "CTable",
  props: {
    columns: {
      type: Array,
      required: true,
    },
    datax: {
      type: Array,
      required: true,
    },
    noDataText: {
      type: String,
      required: false,
      default: 'Không có dữ liệu',
    }
  },
  data() {
    return {
    }
  },
  watch: {},
  computed: {
    hasNoData() {
      return this.datax.length == 0;
    }
  },

  methods: {
    handleClickRow(row) {
      this.$emit('clickRow', row)
    },
    formatData(column, data, difference = null) {
      if (data == null) {
        return '-';
      }
      const { type } = column;
      switch (type) {
        case Enums.EnumColumnType.Number: {
          return data.toLocaleString('en-US');
        }
        case Enums.EnumColumnType.Decimal: {
          return data.toFixed(2).toLocaleString('en-US');
        }
        case Enums.EnumColumnType.Percent: {
          return `${data.toFixed(2)} %`;
        }
        case Enums.EnumColumnType.Time: {
          return this.formatDate(new Date(data));
        }
        case Enums.EnumColumnType.Enum: {
          for (let key in Enums[column.enumName]) {
            if (Enums[column.enumName][key] == data) {
              return this.$t(`i18nEnum.${column.enumName + '_' + key}`);
            }
          }
        }
        default: {
          return data;
        }
      }
    },
    formatDate(date) {
      const hours = date.getHours().toString().padStart(2, '0');
      const minutes = date.getMinutes().toString().padStart(2, '0');
      const day = date.getDate().toString().padStart(2, '0');
      const month = (date.getMonth() + 1).toString().padStart(2, '0');
      const year = date.getFullYear().toString();

      return `${hours}:${minutes} ${day}/${month}/${year}`;
    },
  },
};
</script>

<style lang="scss" scoped>
@import "./ctable.scss";
</style>
