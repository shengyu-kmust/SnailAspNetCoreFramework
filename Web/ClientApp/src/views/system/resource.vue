<template>
  <div>
    <el-row>
      <el-button @click="addParent">添加同级</el-button>
      <el-button @click="addChild">添加子级</el-button>
      <el-button @click="edit">编辑</el-button>
      <el-button @click="remove">删除</el-button>
    </el-row>
    <el-table
      :data="tableData"
      row-key="id"
      border
      default-expand-all
      :tree-props="{children: 'childs'}"
    >
      <el-table-column
        prop="name"
        label="配置名"
      >
      </el-table-column>
      <el-table-column
        prop="key"
        label="键"
        sortable
      >
      </el-table-column>
      <el-table-column
        prop="value"
        label="值"
        sortable
      >
      </el-table-column>
    </el-table>
  </div>
</template>

<script>
export default {
  data() {
    return {
      keyValues: {
        genders: []
      }
    }
  },
  computed: {
  },
  created() {
    this.init()
  },
  methods: {
    init() {
    },
    selectFormatter(row, column, cellValue, index) {
      // return cellValue
      return this.$util.keyValueFormart(this.keyValues.yesnos, cellValue)
    },
    timeFormatter(row, column, cellValue, index) {
      return this.$dayjs(cellValue).format('YYYY-MM-DD')
    }
  }
}
</script>
