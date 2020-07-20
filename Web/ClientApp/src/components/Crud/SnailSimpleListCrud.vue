<template>
  <!-- 不分页的CRUD  -->
  <div style="height:100%;display:flex;flex-direction: column;flex:1" snailscrud>
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
      <!-- table分页 -->
      <el-table
        ref="table"
        border
        v-bind="tableBind"
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
        <template v-for="(field,index) in fields.filter(v=>v.noForTable!=true)">
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

    <!-- form表单 -->
    <el-dialog v-if="visible" :visible.sync="visible">
      <snail-form ref="form" :fields="formFields" :init-form-data="formData" :rules="formRules"></snail-form>
      <template slot="footer">
        <el-button @click="submit">提交</el-button>
        <el-button @click="visible=false">取消</el-button>
      </template>
    </el-dialog>
  </div>
</template>

<script>
import { TableBaseMixin } from '../Table/tableBase.js'
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
    afterSearch: {
      type: Function,
      default: () => { }
    },
    beforeSubmit: {
      type: Function,
      default: () => { }
    },
    handSearchTableDatas: {
      type: Function,
      default: null
    },
    tableBind: {
      type: Object,
      default: () => ({})
    }
  },
  data() {
    return {
      tableDatas: [],
      submitApi: '',
      formData: {},
      visible: false,
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
      var ids = []
      var currentRow = this.currentRow
      var selection = this.selection
      console.log('删除-' + JSON.stringify(currentRow))
      if (this.multiSelect) {
        if (selection.length > 0) {
          ids = selection.map(a => a.id)
        } else {
          this.$message({
            message: '请先选择要删除的数据',
            type: 'warning'
          })
          return
        }
      } else if (currentRow.id) {
        ids = [currentRow.id]
      } else {
        this.$message({
          message: '请先选择要删除的数据',
          type: 'warning'
        })
        return
      }
      this.$api[this.removeApi](ids).then(res => {
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
      this.visible = true
    },
    submit() {
      this.$refs.form.validate(valid => {
        if (valid) {
          if (typeof this.beforeSubmit === 'function') {
            var isContinue = this.beforeSubmit(this.$refs.form.formData)
            if (isContinue === false) {
              return
            }
          }
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
    },
    search() {
      if (this.searchApi) {
        this.$refs.searchForm.validate(valid => {
          if (valid) {
            var serachForm = this.$refs.searchForm.formData
            this.loading = true
            if (typeof this.beforeSearch === 'function') {
              var isContinue = this.beforeSearch(serachForm)
              if (isContinue === false) {
                return
              }
            }
            this.$api[this.searchApi](serachForm).then(res => {
              if (typeof this.handSearchTableDatas === 'function') {
                var data = this.handSearchTableDatas(res)
                if (data) {
                  res.data = data
                }
              }

              if (typeof this.afterSearch === 'function') {
                this.afterSearch()
              }
              this.tableDatas = res.data
            }).finally(() => {
              this.loading = false
            })
          }
        })
      }
    }
  }
}
</script>
<style  lang="scss" scoped>
.el-table{
  /deep/ .el-table__body-wrapper{
    overflow: scroll;
  }
}

</style>
