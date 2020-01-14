<template>
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
</template>

<script>
import { TableBaseMixin } from './tableBase.js'
export default {
  name: 'SnailTable',
  mixins: [TableBaseMixin]

}
</script>
