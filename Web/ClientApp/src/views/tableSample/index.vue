<template>
  <div>
    <p>table示例</p>
    <snail-table :columns="columns" :data="tableData" />
    <!-- <el-table
      :data="tableData"
      style="width: 100%"
    >
      <el-table-column
        prop="string"
        label="字符串"
        width="180"
      />
      <el-table-column
        prop="int"
        label="数字"
        width="180"
      />
      <el-table-column
        prop="datetime"
        label="日期"
      />
      <el-table-column
        v-bind="bindtest"
      />
      <el-table-column
        prop="multiselect"
        label="多下拉"
        :formatter="selectFormat"
      />
    </el-table> -->
  </div>
</template>
<script>
import { getList } from '@/api/table'
import SnailTable from '@/components/Table/SnailTable'
export default {
  components: {
    SnailTable
  },
  data() {
    return {
      tableData: [],
      list: [{ key: 'yes', value: '是' }, { key: 'no', value: '否' }],
      columns: [
        {
          fieldName: 'string',
          label: '字符串'
        },
        {
          fieldName: 'int',
          label: '数字'
        },
        {
          fieldName: 'datetime',
          label: '时间',
          formatter: function(row, column, value) {
            return value.substr(0, 10)
          }
        },
        {
          fieldName: 'select',
          label: '选择'
        },
        {
          fieldName: 'multiselect',
          label: '选择'
        }
      ],
      bindtest: {
        prop: 'select',
        label: '下拉',
        formatter: function(row) {
          return 'formatter test'
        }
      }
    }
  },
  created() {
    getList().then(res => {
      console.log(res.data)
      this.tableData = res.data
    })
    this.columns.find(a => a.fieldName == 'select').keyValues = this.list
    this.columns.find(a => a.fieldName == 'multiselect').keyValues = this.list
    console.log('created')
  },
  methods: {
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
