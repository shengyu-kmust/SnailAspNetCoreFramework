<template>
  <div>
    <slot name="oper">
      <!-- 操作区 -->
    </slot>
    <slot name="search">
      <!-- 查询区下面是默认的 -->
      <snail-search-form ref="searchForm" :fields="searchFields" :rules="searchRules" @search="search" />
    </slot>
    <!-- 查询条件 -->
    <!-- table分页 -->
    <el-table
      ref="table"
      :data="rows"
      :highlight-current-row="highlightCurrentRow"
      @current-change="(currentRow)=>emitEventHandler('current-change',currentRow)"
      @selection-change="(selecttion)=>emitEventHandler('selection-change',selecttion)"
      @row-click="(row, column, event)=>emitEventHandler('row-click',row, column, event)"
    >
      <el-table-column v-if="multiSelect" type="selection"></el-table-column>
      <el-table-column v-if="showTableIndex" type="index" width="50">
        <template slot="header">序号</template>
      </el-table-column>
      <template v-for="(field,index) in fields">
        <el-table-column :key="index" :prop="field.name" :label="field.label" v-bind="field">
          <!-- 如果field的slotName字段有值，则用外部传入的slot来替换column里的template，否则用默认的 -->
          <template v-if="field.slotName" slot-scope="scope">
            <slot :name="field.slotName" :row="scope.row"></slot>
          </template>
        </el-table-column>
      </template>
    </el-table>
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
  </div>
</template>

<script>
import { TableBaseMixin } from './tableBase.js'
export default {
  mixins: [TableBaseMixin],
  props: {
    searchApi: {
      type: String,
      default: () => ('')
    },
    searchFields: {
      type: Array,
      default: () => ([])
    },
    searchRules: {
      type: Object,
      default: () => ([])
    }
  },
  data() {
    return {
      pagination: { pageIndex: 1, pageSize: 15, total: 0 },
      loading: false
    }
  },
  computed: {

  },
  created() {

  },
  mounted() {
    this.search()
  },
  methods: {
    search() {
      this.$refs.searchForm.validate(valid => {
        if (valid) {
          var serachForm = this.$refs.searchForm.formData
          var pagination = this.$refs.table.getPagination()
          var queryData = Object.assign({}, serachForm, pagination)
          this.loading = true
          this.$api[this.searchApi](queryData).then(res => {
            this.pagination.total = parseInt(res.data.total) || 0
            this.pagination.pageSize = parseInt(res.data.pageSize) || 15
            this.pagination.pageIndex = parseInt(res.data.pageIndex) || 1
            this.tableDatas = res.data.items
          }).finally(() => {
            this.loading = false
          })
        }
      })
    }
  }
}
</script>
