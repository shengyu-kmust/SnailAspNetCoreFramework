<template>
  <!-- 分页crud -->
  <div style="height:100%;display:flex;flex-direction: column;flex:1">
    <div>
      <!--默认功能菜单 -->
      <slot v-if="showOper" name="oper">
        <el-button @click="add">增加</el-button>
        <el-button @click="edit">修改</el-button>
        <el-button @click="remove">删除</el-button>
      </slot>
      <!-- 功能菜单扩展插槽 -->
      <slot name="operEx">
      </slot>
      <!-- 查询条件 -->
      <slot v-if="showSearch" name="search">
        <snail-search-form v-show="searchFields.length>0" ref="searchForm" :fields="searchFields" :rules="searchRules" @search="search" />
      </slot>
    </div>

    <div style="flex:1;">
      <el-table
        ref="table"
        border
        :height="tableHeight"
        :data="tableDatas"
        :highlight-current-row="highlightCurrentRow"
        @current-change="(currentRow)=>emitEventHandler('current-change',currentRow)"
        @selection-change="(selecttion)=>emitEventHandler('selection-change',selecttion)"
        @row-click="(row, column, event)=>emitEventHandler('row-click',row, column, event)"
      >
        <el-table-column v-if="multiSelect" type="selection"></el-table-column>
        <el-table-column v-if="showTableIndex" type="index" width="50">
          <template slot="header">
            序号
          </template>
        </el-table-column>
        <template v-for="(field,index) in fields">
          <el-table-column :key="index" :prop="field.name" :label="field.label" v-bind="field">
            <!-- 如果field的slotName字段有值，则用外部传入的slot来替换column里的template，否则用默认的 -->
            <template slot-scope="scope">
              <slot v-if="field.slotName" :name="field.slotName" :row="scope.row"></slot>
              <span v-else-if="field.formatter">{{ field.formatter(scope.row,scope.column, scope.row[field.name], scope.$index) }}</span>
              <span v-else-if="field.type==='select' && field.keyValues">{{ $util.keyValueFormart(field.keyValues, scope.row[field.name]) }}</span>
              <span v-else>{{ scope.row[field.name] }}</span>
            </template>
          </el-table-column>
        </template>
      </el-table>
    </div>
    <el-pagination
      ref="pagination"
      style="margin-top: 10px;text-align: right;"
      :current-page="pagination.pageIndex"
      :page-size="pagination.pageSize"
      :total="pagination.total"
      :page-sizes="pageSizes"
      :layout="layout"
      @size-change="(pageSize)=>emitEventHandler('pagination-size-change',pageSize)"
      @current-change="(pageIndex )=>emitEventHandler('pagination-current-change',pageIndex )"
      @next-click="(pageIndex)=>emitEventHandler('pagination-next-click',pageIndex)"
      @prev-click="(pageIndex)=>emitEventHandler('pagination-prev-click',pageIndex)"
    ></el-pagination>
    <!-- table分页 -->
    <!-- 这一段和 snailTable是一样的-->
    <!-- form表单 -->
    <el-dialog v-if="visible" :visible.sync="visible">
      <slot :formData="formData" name="form">
        <snail-form ref="form" :fields="formFields" :init-form-data="formData" :rules="formRules"></snail-form>
      </slot>
      <template slot="footer">
        <el-button @click="submit">提交</el-button>
        <el-button @click="visible=false">取消</el-button>
      </template>
    </el-dialog>

  </div>
</template>

<script>
import { TableBaseMixin } from '../Table/tableBase'

export default {
  mixins: [TableBaseMixin],
  props: {
    autoLoad: {
      type: Boolean,
      default: true
    },
    showOper: {
      type: Boolean,
      default: true
    },
    showSearch: {
      type: Boolean,
      default: true
    },
    searchApi: {
      type: String,
      default: () => ('')
    },
    addApi: {
      type: String,
      default: () => ('')
    },
    editApi: {
      type: String,
      default: () => ('')
    },
    removeApi: {
      type: String,
      default: () => ('')
    },
    searchFields: {
      type: Array,
      default: () => ([])
    },
    formFields: {
      type: Array,
      default: () => ([])
    },
    searchRules: {
      type: Object,
      default: () => ({})
    },
    formRules: {
      type: Object,
      default: () => ({})
    },
    beforeSearch: {
      type: Function,
      default: () => { }
    },
    beforeSubmit: {
      type: Function,
      default: () => { }
    },
    // 完全自己定义提交逻辑
    submitHandler: {
      type: Function,
      default: () => { }
    },
    // 下面是snailPageTable里的Props
    pagination: {
      type: Object,
      default: () => {
        return {
          pageIndex: 1,
          pageSize: 15,
          total: 0
        }
      }
    },
    pageSizes: {
      type: Array,
      default: () => ([15, 30, 50, 100, 200])
    },
    layout: {
      type: String,
      default: () => ('total, prev, pager, next, jumper, sizes')
    }
  },
  data() {
    return {
      tableDatas: [],
      submitApi: '',
      formData: {},
      visible: false,
      // pagination: {pageIndex: 1, pageSize: 15, total: 0},
      loading: false
    }
  },
  computed: {

  },
  created() {

  },
  mounted() {
    if (this.autoLoad) {
      this.search()
    }
  },
  methods: {
    remove() {
      // 为适应删除1个和多个，都传到后台一个数组，后台接收的预约不变
      var data = {}
      var currentRow = this.currentRow
      var selection = this.selection
      console.log('删除-' + JSON.stringify(currentRow))
      if (this.multiSelect) {
        if (selection.length > 0) {
          data = selection.map(a => a.id)
        } else {
          this.$message({
            message: '请先选择要删除的数据',
            type: 'warning'
          })
          return
        }
      } else if (currentRow.id) {
        data = [currentRow.id]
      } else {
        this.$message({
          message: '请先选择要删除的数据',
          type: 'warning'
        })
        return
      }
      console.log(data)
      this.$api[this.removeApi](data).then(res => {
        this.$message({
          message: '操作成功',
          type: 'success'
        })
        this.search()
      }).catch(res => {
        this.$message({
          message: '操作失败',
          type: 'error'
        })
      })
    },
    add() {
      this.submitApi = this.addApi
      this.formData = {}
      this.visible = true
    },
    edit() {
      if (this.multiSelect && this.selection.length > 1) {
        this.$message({
          message: '只能对一条数据进行修改',
          type: 'warning'
        })
        return
      }
      if (!this.multiSelect && !this.currentRow.id) {
        this.$message({
          message: '请选择数据后再进行修改',
          type: 'warning'
        })
        return
      }
      this.submitApi = this.editApi
      this.formData = this.currentRow
      console.log('edit-' + JSON.stringify(this.formData))
      this.visible = true
    },
    submit() {
      if (typeof this.submitHandler === 'function') {
        this.submitHandler()
      } else {
        console.log(this.$refs.form.formData)
        this.$refs.form.validate(valid => {
          if (valid) {
            this.$api[this.submitApi](this.$refs.form.formData).then(res => {
              this.$message({
                message: '操作成功',
                type: 'success'
              })
              this.search()
            }).catch(res => {
              this.$message({
                message: '操作失败',
                type: 'error'
              })
            })
            this.visible = false
          }
        })
      }
    },
    search() {
      this.$refs.searchForm.validate(valid => {
        if (valid) {
          var serachForm = this.$refs.searchForm.formData
          var pagination = this.getPagination()
          if (typeof this.beforeSearch === 'function') {
            var isContinue = this.beforeSearch(serachForm)
            if (isContinue === false) {
              return
            }
          }
          var queryData = Object.assign({}, serachForm, pagination)
          console.log(queryData)
          this.loading = true
          this.$api[this.searchApi](queryData).then(res => {
            // this.pagination.total = parseInt(res.data.total) || 0;
            // this.pagination.pageSize = parseInt(res.data.pageSize) || 15;
            // this.pagination.pageIndex = parseInt(res.data.pageIndex) || 1;
            // this.tableDatas = res.data.items;

            // todo 改成自己的，上面
            this.pagination.total = parseInt(res.data.totalItemCount) || 0
            this.pagination.pageSize = parseInt(res.data.pageSize) || 15
            this.pagination.pageIndex = parseInt(res.data.pageNumber) || 1
            this.tableDatas = res.data.items
          }).finally(() => {
            this.loading = false
          })
        }
      })
    },
    // 这个是SnailPageTable里的方法
    getPagination() {
      return {
        pageIndex: this.$refs.pagination.internalCurrentPage,
        pageSize: this.$refs.pagination.internalPageSize
      }
    },
    emitEventHandler(event) { // 对所有table的事件进行监听，并向父组件抛事件，
      if (['pagination-size-change', 'pagination-current-change', 'pagination-next-click', 'pagination-prev-click'].indexOf(event) > -1) {
        this.search()
      }
      if (event === 'row-click') { // 点击
        console.log('row-click' + JSON.stringify(arguments))
        var table = this.$refs[this.tableRefName]
        table.toggleRowSelection(Array.from(arguments)[1])
      }
      if (event === 'current-change') { // 单选点击
        this.currentRow = Array.from(arguments)[1]
      }

      if (event === 'selection-change') { // 多选点击
        this.selection = Array.from(arguments)[1]
      }

      this.$emit(event, ...Array.from(arguments).slice(1))// 所有事件往上抛，让外部也能监听
    }
  }
}
</script>
