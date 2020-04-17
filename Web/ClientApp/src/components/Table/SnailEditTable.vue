<!--
行内编辑组件
fields:{
  name:'',
  type:'',//支持：string,int,datetime,date,select,multiSelect
  label:'',
  keyValues:[],//
  color:'',//颜色
}

-->
<template>
  <div>
    <el-table :data="rows">
      <el-table-column v-for="(field,index) in fields"
                       :prop="field.name"
                       :label="field.label"
                       v-bind="field"
                       :key="index">
        <template slot-scope="scope">
          <div v-if="scope.row.isEdit">
            <!-- 输入 -->
            <el-input v-if="field.type==='string'" v-model="editRow[field.name]" v-bind="field" />
            <!-- 时间 -->
            <el-date-picker v-if="field.type==='datetime'"
                            v-model="editRow[field.name]"
                            style="width:100%"
                            type="datetime"
                            v-bind="field" />
            <!-- 日期 -->
            <el-date-picker v-if="field.type==='date'"
                            v-model="editRow[field.name]"
                            style="width:100%"
                            type="date"
                            v-bind="field" />
            <!-- 单选 -->
            <el-select v-if="field.type==='select'"
                       v-model="editRow[field.name]"
                       style="width:100%"
                       v-bind="field">
              <el-option v-for="keyValue in field.keyValues"
                         :key="keyValue[field.key || 'key']"
                         :label="keyValue[field.value || 'value']"
                         :value="keyValue[field.key || 'key']"></el-option>
            </el-select>
            <!-- 多选 -->
            <el-select v-if="field.type==='multiSelect'"
                       v-model="editRow[field.name]"
                       multiple
                       style="width:100%"
                       v-bind="field">
              <el-option v-for="keyValue in field.keyValues"
                         :key="keyValue[field.key || 'key']"
                         :label="keyValue[field.key || 'value']"
                         :value="keyValue[field.key || 'key']"></el-option>
            </el-select>
            <!-- 数字 -->
            <el-input-number v-if="field.type==='int'" v-model="editRow[field.name]" v-bind="field"></el-input-number>
            <!-- 颜色 -->
            <el-color-picker v-if="field.type==='color'" v-model="editRow[field.name]" v-bind="field"></el-color-picker>
          </div>
          <div v-else>
            <el-color-picker disabled='true' v-if="field.type==='color'" v-model="scope.row[field.name]" v-bind="field"></el-color-picker>
            <div v-else>
              {{formart(scope.row,field)}}
            </div>
          </div>
        </template>
      </el-table-column>
      <el-table-column label="操作" width="150" v-if="enableEdit">
        <template slot-scope="scope">
          <el-button size="mini" style="margin:0px" @click="saveOrEdit(scope.row)">{{scope.row.isEdit?'保存':'修改'}}</el-button>
          <el-button size="mini" style="margin:0px" @click="removeOrCancel(scope.row)">{{scope.row.isEdit?'取消':'删除'}}</el-button>
        </template>
      </el-table-column>
    </el-table>
    <el-button style="width:100%" @click="add">添加</el-button>
  </div>
</template>

<script>
  import { keyValueFormart } from '@/utils';

  export default {
    name: 'SnailEditTable',
    props: {
      fields: {
        // table的所有字段
        type: Array,
        default: () => []
      },
      rows: {
        // table的所有初始数据
        type: Array,
        default: () => []
      },
      saveHandler: {
        // 保存方法，默认不调用后台接口，请输入此函数以保存数据到后台
        type: Function,
        default: (row) => { return true; }
      },
      removeHandler: {
        // 保存方法，默认不调用后台接口，请输入此函数以从后台删除数据
        type: Function,
        default: (row) => { return true; }
      },
      enableEdit: {
        type: Boolean,
        default: true
      }
    },
    data() {
      return {
        editRow: {

        },
      };
    },
    computed() {
    },
    methods: {
      saveOrEdit(row) {
        if (row.isEdit) {
          var saveResult = this.saveHandler(row);
          if (saveResult) {
            if (typeof (saveResult) === 'object') {
              Object.assign(row, saveResult);
              row.isEdit = false;
            } else if (saveResult === true) {
              Object.assign(row, this.editRow);
              row.isEdit = false;
            }
          }
        } else {
          if (this.rows.filter(a => a.isEdit).length >= 1) {
            return;
          }
          this.editRow = Object.assign({}, row);
          row.isEdit = true;
        }
      },
      removeOrCancel(row) {
        if (row.isEdit) {
          row.isEdit = false;
        } else if (this.removeHandler(row)) {
          var index = this.rows.indexOf(row);
          this.rows.splice(index, 1);
        }
      },
      add() {
        if (this.rows.filter(a => a.isEdit).length >= 1) {
          return;
        }
        this.rows.push({ isEdit: true });
        this.editRow = { isEdit: true };
      },
      getRows() {
        return this.rows;
      },
      formart(row, field) {
        // debugger;
        if (field.formatter && typeof (field.formatter) === 'function') {
          return field.formatter(row, field);
        } else if (field.type === 'select' || field.type === 'multiSelect') {
          return keyValueFormart(field.keyValues, row[field.name]);
        } else if (field.type === 'date' && (row[field.name] instanceof Date)) {
          return row[field.name].toISOString().substr(0, 10);
        } else if (field.type === 'datetime' && (row[field.name] instanceof Date)) {
          return row[field.name].toISOString().substr(0, 19).replace('T', ' ');
        }
        return row[field.name];
      }


    }
  };
</script>

<style>
</style>
