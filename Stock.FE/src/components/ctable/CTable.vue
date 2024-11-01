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
      <!-- <tbody class="tbody">
        <tr v-for="(item, index) in datax" :key="index">
          <td v-for="(column, index) in columns" :key="index" :style="{
            'text-align': column.textAlign,
            width: column.width ? column.width + 'px' : null,
            minWidth: column.width ? column.width + 'px' : null,
          }" :ref="column.field" @click="handleClickRow(item)">
            <span :class="`td__content ${computedColor(item.difference, column)}`"> {{ formatData(column.type,
              item[column.field], item.difference)
              }}</span>
          </td>
        </tr>
      </tbody> -->
      <tbody class="tbody">

        <div class="no-data">
          <!-- <div class="no-data-img"> -->
          <img src="@/assets/img/no-data.png" alt="">
          <!-- </div> -->
          <!-- <span class="no-data-text">{{ noDataText }}</span> -->
        </div>
      </tbody>
    </table>
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

  methods: {
    handleClickRow(row) {
      this.$emit('clickRow', row)
    },
    formatData(type, data, difference = null) {
      if (data == null) {
        return '-';
      }
      switch (type) {
        case Enums.EnumColumnType.Number: {
          return data.toLocaleString('en-US');
        }
        case Enums.EnumColumnType.Percent: {
          if (difference !== null && difference !== undefined) {

            if (difference == 0) {
              return `+${data}%`
            } else if (difference == 1) {
              return `-${data}%`
            } else {
              return `${data}%`
            }
          }
          break;
        }
        case Enums.EnumColumnType.Time: {
          return this.formatDate(new Date(data));
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
      const year = date.getFullYear().toString().slice(2);

      return `${hours}:${minutes} ${day}/${month}/${year}`;
    },

    computedColor(difference, column) {
      if (column.isColor) {

        switch (difference) {
          case 0:
            return 'green';
          case 1:
            return 'red';
          case 2:
            return 'yellow';
          default:
            break;
        }
      } else {
        return 'white';
      }
    }
  },
};
</script>

<style lang="scss" scoped>
@import "./ctable.scss";
</style>
