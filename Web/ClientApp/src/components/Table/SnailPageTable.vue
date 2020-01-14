<template>
  <!-- 这一段和 snailTable是一样的-->
  <div>
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
        <el-table-column :key="index" :prop="field.name" :label="field.label" v-bind="field" />
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
      @current-change="(currentPage )=>emitEventHandler('pagination-current-change',currentPage )"
      @next-click="(currentPage)=>emitEventHandler('pagination-next-click',currentPage)"
      @prev-click="(currentPage)=>emitEventHandler('pagination-prev-click',currentPage)"
    ></el-pagination>
  </div>
</template>

<script>
import { TableBaseMixin } from './tableBase.js'
export default {
  name: 'SnailPageTable',
  mixins: [TableBaseMixin],
  props: {
    pagination: {
      type: Object,
      default: () => {
        return {
          currentPage: 1,
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
  methods: {
    getPagination() {
      return {
        currentPage: this.$refs.pagination.internalCurrentPage,
        pageSize: this.$refs.pagination.internalPageSize
      }
    }

  }
}
</script>
