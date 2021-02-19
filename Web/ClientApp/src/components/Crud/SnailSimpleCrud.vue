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
import { simpleCrudBaseMixin } from './simpleCrudBase.js'

export default {
    mixins: [TableBaseMixin,simpleCrudBaseMixin],
}
</script>
