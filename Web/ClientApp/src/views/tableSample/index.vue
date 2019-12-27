<template>
  <div>
    <p>table示例</p>
    <el-table
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
        prop="select"
        label="下拉"
        :formatter="selectFormat"
      />
      <el-table-column
        prop="multiselect"
        label="多下拉"
      />
    </el-table>
  </div>
</template>
<script>
import { getList } from '@/api/table'
export default {
  data() {
    return {
      tableData: [],
      list: [{ key: 'yes', value: '是' }, { key: 'yes', value: '是' }]
    }
  },
  created() {
    getList().then(res => {
      console.log(res.data)
      this.tableData = res.data
    })
  },
  methods: {
    selectFormat(val) {
      if (Array.isArray(val)) {
        return this.list.filter(a => val.indexOf(a.key) > -1).map(val => val.value).join(',')
      } else {
        return this.list.find(a => a.key === val.select).value
      }
    //   return 'test'
    }
  }
}
</script>
