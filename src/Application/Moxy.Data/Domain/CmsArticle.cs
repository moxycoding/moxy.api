﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Moxy.Data.Domain
{
    [Table("cms_article")]
    public partial class CmsArticle : BaseEntity<int>
    {
        /// <summary>
        /// 友好地址名
        /// </summary>
        [Column("display_code"), MaxLength(100)]
        public string EntryName { get; set; }
        /// <summary>
        /// 文章作者
        /// </summary>
        [Column("art_author"), MaxLength(100)]
        public string ArtAuthor { get; set; }
        /// <summary>
        /// 文章标题
        /// </summary>
        [Column("art_title"), MaxLength(200)]
        public string ArtTitle { get; set; }
        /// <summary>
        /// 文章内容
        /// </summary>
        [Column("art_content")]
        public string ArtContent { get; set; }
        /// <summary>
        /// 文章简介
        /// </summary>
        [Column("art_desc"), MaxLength(300)]
        public string ArtDesc { get; set; }
        /// <summary>
        /// 文章标签
        /// </summary>
        [Column("tags"), MaxLength(300)]
        public string Tags { get; set; }
        /// <summary>
        /// 是否发布
        /// </summary>
        [Column("is_release")]
        public bool IsRelease { get; set; }
        /// <summary>
        /// 是否发布
        /// </summary>
        [Column("is_set_top")]
        public bool? IsSetTop { get; set; }
        /// <summary>
        /// 发布时间
        /// </summary>
        [Column("release_time")]
        public DateTime? ReleaseTime { get; set; }

        /// <summary>
        /// 分类
        /// </summary>
        [Column("category_id")]
        public int? CategoryId { get; set; }
    }
}
