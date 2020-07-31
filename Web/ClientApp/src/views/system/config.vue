<template>
  <div style="width:100%">
    <!-- element 2.8.2及以上才有树table -->
    <snail-simple-list-crud
      ref="table"
      :show-table-index="false"
      search-api="configQueryListTree"
      add-api="configSave"
      edit-api="configSave"
      :fields="fields"
      :table-bind="tableBind"
      :form-fields="fields"
      :before-submit="beforeSubmit"
      :after-search="afterSearch"
      @current-change="currentChange"
    >
      <template slot="oper">
        <div>
          <el-row>
            <el-button @click="addParent">添加同级</el-button>
            <el-button @click="addChild">添加子级</el-button>
            <el-button @click="edit">编辑</el-button>
            <el-button @click="remove">删除</el-button>
            <el-button @click="refresh">刷新</el-button>
          </el-row>
        </div>
      </template>
    </snail-simple-list-crud>
  </div>
</template>

<script>
export default {
  data() {
    return {
      keyValues: {
        genders: []
      },
      currentRow: {},
      addParentId: '',
      rootId: '',
      tableData: [],
      fields: [
        {
          name: 'key',
          type: 'string',
          label: '键'
        },
        {
          name: 'value',
          type: 'string',
          label: '值'
        },
        {
          name: 'name',
          type: 'string',
          label: '描述'
        }
      ],
      tableBind: {
        rowKey: 'id',
        defaultExpandAll: true
      }
    }
  },
  computed: {
  },
  created() {
    this.init()
  },
  methods: {
    afterSearch() {
      try {
        var data = this.$refs.table.$refs.table.data
        if (data) {
          this.$refs.table.$refs.table.toggleRowExpansion(data.find(a => a.id == this.addParentId), true)
        }
      } catch {

      }
    },
    currentChange(row) {
      this.currentRow = row
    },
    addParent() {
      if (this.currentRow && this.currentRow.parentId) {
        this.addParentId = this.currentRow.parentId
      } else {
        this.addParentId = this.rootId
      }
      console.log(this.addParentId)
      this.$refs.table.add()
    },
    refresh() {
      this.$refs.table.search()
    },
    addChild() {
      this.addParentId = this.currentRow.id
      if (!this.addParentId) {
        this.$message.error('请先选择数据，再对其增加子级')
        return
      }
      console.log(this.addParentId)
      this.$refs.table.add()
    },
    beforeSubmit(formData) {
      formData.parentId = this.addParentId
    },
    edit() {
      this.addParentId = this.currentRow.parentId
      this.$refs.table.edit()
    },
    init() {
    },
    remove() {
      this.$refs.table.remove()
    },
    handSearchTableDatas(res) {
      var data = res.data
      this.rootId = data.data.id
      this.convert(data)
      return data.children
    },
    convert(data) {
      Object.assign(data, data.data)
      data.children = data.childs
      delete data.childs
      delete data.parent
      delete data.data
      if (data && data.children && data.children.length > 0) {
        data.hasChildren = true
        data.children.forEach(item => {
          this.convert(item)
        })
      } else {
        data.hasChildren = false
      }
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
