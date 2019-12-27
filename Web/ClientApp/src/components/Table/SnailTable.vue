<template>
  <el-table :data="data">
    <template v-for="column in columns">
      <el-table-column
        :prop="column.fieldName"
        :label="column.label"
        :formatter="column.formatter || formatter"
      />

    </template>
  </el-table>
</template>

<script>
export default {
  name: 'SnailTable',
  props: {
    columns: Array,
    data: Array
  },
  methods: {
    formatter(row, column, cellValue) {
      console.log('formatter')
      var keyValues = this.columns.find(a => a.fieldName === column.property).keyValues
      if (keyValues) {
        if (Array.isArray(cellValue)) {
          return keyValues.filter(a => cellValue.indexOf(a.key) > -1).map(val => val.value).join(',')
        } else {
          return keyValues.find(a => a.key === cellValue).value
        }
      } else {
        return cellValue
      }
    },
    selectFormat(row, column, cellValue) {
      if (Array.isArray(cellValue)) {
        return this.list.filter(a => cellValue.indexOf(a.key) > -1).map(val => val.value).join(',')
      } else {
        return this.list.find(a => a.key === cellValue).value
      }
    }
  }
}
</script>
